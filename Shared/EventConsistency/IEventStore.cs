using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;

namespace LabApp.Shared.EventConsistency
{
    public interface IEventStore
    {
        public Task AddAsync(IIntegrationEvent @event);
        public Task<EventMessage> GetEventAsync(string id);
        public Task<IEnumerable<EventMessage>> GetEventsAsync(IEnumerable<string> ids);
        public Task<bool> TryDeleteEventAsync(EventMessage message);
        public Task DeleteEventsAsync(IEnumerable<EventMessage> messages);
        public Task<IEnumerable<EventMessage>> GetUnPublishedAsync(TimeSpan maxTime);
    }

    public interface IInboxEventStore : IEventStore
    {
        Task IncrementFailedAsync(EventMessage eventMessage);
    }
    
    public interface IOutboxEventStore : IEventStore
    {
    }
}