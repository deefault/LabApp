using System.Collections.Generic;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public interface IEventPublisher
    {
        public Task PublishAsync(IntegrationEvent @event);
        public Task PublishAsync(IEnumerable<IntegrationEvent> events);
    }
}