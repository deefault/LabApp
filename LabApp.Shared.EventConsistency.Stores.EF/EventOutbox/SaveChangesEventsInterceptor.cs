using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LabApp.Shared.EventConsistency.Stores.EF.EventOutbox
{
    //TODO
    public class SaveChangesEventsInterceptor : SaveChangesInterceptor
    {
        private readonly IOutboxListener _listener;
        
        public SaveChangesEventsInterceptor(IOutboxListener listener)
        {
            _listener = listener;
        }
        
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {   
            IContextWithEventOutbox contextWithEventOutbox = GuardCast(eventData);
            if (contextWithEventOutbox.SaveEventsOnSaveChanges)
            {
                var events = GetEvents(eventData.Context);
                if (events.Any())
                {
                    int intResult = 0;
                    using (var transaction = new TransactionScope(new CommittableTransaction(new TransactionOptions()
                        {IsolationLevel = IsolationLevel.ReadCommitted})))
                    {
                        intResult = eventData.Context.SaveChanges();
                        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                        contextWithEventOutbox.EventOutbox.AddRange(events); 
                        eventData.Context.SaveChanges();

                        transaction.Complete();
                    }

                    OnEventsAdded(events.Select(x => x.Id.ToString()));
                    return InterceptionResult<int>.SuppressWithResult(intResult);
                }
            }

            return new InterceptionResult<int>();
        }
        
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = new CancellationToken())
        {
            IContextWithEventOutbox contextWithEventOutbox = GuardCast(eventData);
            if (contextWithEventOutbox.SaveEventsOnSaveChanges)
            {
                var events = GetEvents(eventData.Context);
                if (events.Any())
                {
                    int intResult = 0;
                    using (var transaction = new TransactionScope(new CommittableTransaction(new TransactionOptions()
                        {IsolationLevel = IsolationLevel.ReadCommitted})))
                    {
                        intResult = await eventData.Context.SaveChangesAsync(cancellationToken);
                        // ReSharper disable once MethodHasAsyncOverloadWithCancellation
                        contextWithEventOutbox.EventOutbox.AddRange(events);
                        await eventData.Context.SaveChangesAsync(cancellationToken);

                        transaction.Complete();
                    }

                    OnEventsAdded(events.Select(x => x.Id.ToString()));
                    return InterceptionResult<int>.SuppressWithResult(intResult);
                }
            }

            return new InterceptionResult<int>();
        }
        
        
        private void OnEventsAdded(IEnumerable<string> integrationEvents)
        {
            _listener.OnNewMessages(integrationEvents);
        }

        private IReadOnlyList<OutboxEventMessage> GetEvents(DbContext dbContext)
        {
            var eventsToSave = dbContext.ChangeTracker.Entries().Select(x => x.Entity)
                .Where(x => x is EventEntity {Events: { }}).Cast<EventEntity>().SelectMany(x => x.Events);
            return eventsToSave.Select(x => new OutboxEventMessage()
            {
                Id = x.Id.ToString(),
                DateTime = DateTime.UtcNow,
                EventData = x
            }).ToList();
        }
        
        private IContextWithEventOutbox GuardCast(DbContextEventData data)
        {
            if (!(data.Context is IContextWithEventOutbox result))
            {
                throw new NotSupportedException(
                    $"Please inherit {nameof(DbContextWithEvents)} or implement {nameof(IContextWithEventOutbox)}");
            }

            return result;
        }
    }
}