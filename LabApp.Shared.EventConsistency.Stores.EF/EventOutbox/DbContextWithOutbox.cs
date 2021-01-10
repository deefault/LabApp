using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF.EventOutbox
{
    public abstract class DbContextWithOutbox : DbContext, IContextWithEventOutbox
    {
        private readonly IOutboxListener _listener;
        
        public bool SaveEventsOnSaveChanges { get; set; } = true;
        public DbContext Context => this;
        public abstract DbSet<OutboxEventMessage> EventOutbox { get; set; }

        protected DbContextWithOutbox(DbContextOptions options, IOutboxListener listener) : base(options)
        {
            _listener = listener;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (Database.IsRelational() && (!Database.IsNpgsql() || !Database.IsSqlServer()))
            {
                throw new NotSupportedException("only Npgsql and SqlServer providers are supported");
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.AddEventConsistency(Database);
        }

        public override int SaveChanges()
        {
            if (SaveEventsOnSaveChanges)
            {
                var events = GetEvents();
                if (events.Any())
                {
                    return SaveChangesWithEvents(events);
                }
            }

            return base.SaveChanges();
        }

        private int SaveChangesWithEvents(IReadOnlyList<OutboxEventMessage> events)
        {
            int result = 0;
            using (var transaction = new TransactionScope(new CommittableTransaction(new TransactionOptions()
                {IsolationLevel = IsolationLevel.ReadCommitted})))
            {
                result = base.SaveChanges();
                EventOutbox.AddRange(events);
                base.SaveChanges();

                transaction.Complete();
            }

            OnEventsAdded(events.Select(x => x.Id.ToString()));

            return result;
        }

        private async Task<int> SaveChangesWithEventsAsync(IReadOnlyList<OutboxEventMessage> events,
            bool acceptChanges = true, CancellationToken ct = default)
        {
            int result = 0;
            using (var transaction = new TransactionScope(new CommittableTransaction(new TransactionOptions()
                {IsolationLevel = IsolationLevel.ReadCommitted})))
            {
                result = await base.SaveChangesAsync(acceptChanges, ct);
                EventOutbox.AddRange(events);
                await base.SaveChangesAsync(acceptChanges, ct);

                transaction.Complete();
            }

            OnEventsAdded(events.Select(x => x.Id.ToString()));

            return result;
        }

        public Task<int> SaveChangesAsync()
        {
            return SaveChangesAsync(CancellationToken.None);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            if (SaveEventsOnSaveChanges)
            {
                var events = GetEvents();
                if (events.Any())
                {
                    return SaveChangesWithEventsAsync(events, ct: cancellationToken);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken)
        {
            if (SaveEventsOnSaveChanges)
            {
                var events = GetEvents();
                if (events.Any())
                {
                    return SaveChangesWithEventsAsync(events, ct: cancellationToken,
                        acceptChanges: acceptAllChangesOnSuccess);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void OnEventsAdded(IEnumerable<string> integrationEvents)
        {
            _listener.OnNewMessages(integrationEvents);
        }

        private IReadOnlyList<OutboxEventMessage> GetEvents()
        {
            var eventsToSave = ChangeTracker.Entries().Select(x => x.Entity)
                .Where(x => x is EventEntity {Events: { }}).Cast<EventEntity>().SelectMany(x => x.Events);
            return eventsToSave.Select(x => new OutboxEventMessage()
            {
                Id = x.Id.ToString(),
                DateTime = DateTime.UtcNow,
                EventData = x
            }).ToList();
        }
    }
}