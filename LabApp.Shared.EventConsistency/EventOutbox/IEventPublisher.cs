using System.Collections.Generic;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public interface IEventPublisher
    {
        public Task PublishAsync(BaseIntegrationEvent @event);
        public Task PublishAsync(IEnumerable<BaseIntegrationEvent> events);
    }
}