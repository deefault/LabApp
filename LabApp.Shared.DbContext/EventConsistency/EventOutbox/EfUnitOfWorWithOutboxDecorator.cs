using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.Data;
using LabApp.Shared.DbContext.Models;
using LabApp.Shared.DbContext.UnitOfWork;
using LabApp.Shared.EventConsistency;
using LabApp.Shared.EventConsistency.EventOutbox;

namespace LabApp.Shared.DbContext.EventConsistency.EventOutbox
{
    public class EfUnitOfWorWithOutboxDecorator<TContext> : IUnitOfWork where TContext : Microsoft.EntityFrameworkCore.DbContext, IContextWithEventOutbox
    {
        private readonly EfUnitOfWork<TContext> _uw;
        private readonly IOutboxListener _listener;

        public EfUnitOfWorWithOutboxDecorator(EfUnitOfWork<TContext> uw, IOutboxListener listener)
        {
            _uw = uw;
            _listener = listener;
        }

        public TransactionScope BeginTransaction(IsolationLevel isolationLevel) => _uw.BeginTransaction(isolationLevel);

        public void SaveChanges()
        {
            var events = GetEvents();
            if (events.Any())
            {
                using (TransactionScope transaction = BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    _uw.SaveChanges();
                    _uw.DbContext.EventOutbox.AddRange(events);
                    _uw.SaveChanges();

                    transaction.Complete();
                }

                OnEventsAdded(events.Select(x => x.Id.ToString()));
            }
            else
            {
                _uw.SaveChanges();
            }
        }

        private void OnEventsAdded(IEnumerable<string> integrationEvents)
        {
            _listener.OnNewMessages(integrationEvents);
        }

        public async Task SaveChangesAsync()
        {
            var events = GetEvents();
            if (events.Any())
            {
                using (TransactionScope transaction = BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    await _uw.SaveChangesAsync();
                    _uw.DbContext.EventOutbox.AddRange(events);
                    await _uw.SaveChangesAsync();

                    transaction.Complete();
                }
                
                OnEventsAdded(events.Select(x => x.Id.ToString()));
            }
            else
            {
                await _uw.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync(CancellationToken token)
        {
            var events = GetEvents();
            if (events.Any())
            {
                using (TransactionScope transaction = BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    await _uw.SaveChangesAsync(token);
                    _uw.DbContext.EventOutbox.AddRange(events);
                    await _uw.SaveChangesAsync(token);

                    transaction.Complete();
                }
                
                OnEventsAdded(events.Select(x => x.Id.ToString()));
            }
            else
            {
                await _uw.SaveChangesAsync(token);
            }
        }

        private IReadOnlyList<OutboxEventMessage> GetEvents()
        {
            var eventsToSave = _uw.DbContext.ChangeTracker.Entries().Select(x => x.Entity)
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