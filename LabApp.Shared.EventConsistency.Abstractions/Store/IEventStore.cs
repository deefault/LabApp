using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IEventStore
    {
        Task AddAsync(BaseIntegrationEvent @event);
        Task<EventMessage> GetEventAsync(string id);
        Task<IEnumerable<EventMessage>> GetEventsAsync(IEnumerable<string> ids);
        Task<bool> TryDeleteEventAsync(EventMessage message);
        Task DeleteEventsAsync(IEnumerable<EventMessage> messages);
        Task<IEnumerable<EventMessage>> GetUnpublishedAsync(TimeSpan maxTime);
    }
}