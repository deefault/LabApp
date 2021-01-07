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
        private readonly DbContextWithEvents _db;
        
        public EfInboxEventStore(DbContextWithEvents db)
        {
            _db = db;
        }

        public async Task AddAsync(BaseIntegrationEvent @event)
        {
            _db.EventOutbox.Add(@event.ToEventMessage());
            await _db.SaveChangesAsync();
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
            _db.Update(message);
            return (await _db.SaveChangesAsync()) == 1;
        }

        public async Task DeleteEventsAsync(IEnumerable<EventMessage> messages)
        {
            foreach (var message in messages)
            {
                message.DateDelete = DateTime.UtcNow;
                _db.Update(message);
            }
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<EventMessage>> GetUnpublishedAsync(TimeSpan maxTime)
        {
            DateTime toDate = DateTime.UtcNow - maxTime;
            
            return await _db.EventInbox.Where(x => x.DateDelete == null && x.DateTime >= toDate).ToListAsync();
        }

        public async Task IncrementFailedAsync(InboxEventMessage eventMessage)
        {
            eventMessage.FailedProcessingCount++;
            _db.Update(eventMessage);
            await _db.SaveChangesAsync();
        }
    }
}