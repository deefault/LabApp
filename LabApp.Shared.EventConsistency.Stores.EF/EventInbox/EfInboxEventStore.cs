using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF.EventInbox
{
    public class EfInboxEventStore : IInboxEventStore
    {
        private readonly IContextWithEventInbox _db;

        public EfInboxEventStore(IContextWithEventInbox db)
        {
            _db = db;
        }

        public async Task AddAsync(BaseIntegrationEvent @event)
        {
            _db.EventInbox.Add(@event.ToInboxEventMessage());
            await _db.Context.SaveChangesAsync();
        }

        public async Task<EventMessage> GetEventAsync(string id)
        {
            return await _db.EventInbox.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<EventMessage>> GetEventsAsync(IEnumerable<string> ids)
        {
            return await _db.EventInbox.Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> TryDeleteEventAsync(EventMessage message)
        {
            message.DateDelete = DateTime.UtcNow;
            _db.Context.Update(message);
            return (await _db.Context.SaveChangesAsync()) == 1;
        }

        public async Task DeleteEventsAsync(IEnumerable<EventMessage> messages)
        {
            foreach (var message in messages)
            {
                message.DateDelete = DateTime.UtcNow;
                _db.Context.Update(message);
            }
            await _db.Context.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventMessage>> GetUnpublishedAsync(TimeSpan maxTime)
        {
            DateTime toDate = DateTime.UtcNow - maxTime;
            
            return await _db.EventInbox.Where(x => x.DateDelete == null && x.DateTime >= toDate).ToListAsync();
        }

        public async Task IncrementFailedAsync(InboxEventMessage eventMessage)
        {
            eventMessage.FailedProcessingCount++;
            _db.Context.Update(eventMessage);
            await _db.Context.SaveChangesAsync();
        }
    }
}