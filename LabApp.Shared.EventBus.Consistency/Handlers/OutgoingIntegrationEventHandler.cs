using System;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Interfaces;
using LabApp.Shared.EventConsistency.Abstractions;

namespace LabApp.Shared.EventBus.Consistency
{
    public class OutgoingIntegrationEventHandler : IOutgoingIntegrationEventHandler<IntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        
        public OutgoingIntegrationEventHandler(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task HandleAsync(IntegrationEvent @event)
        {
            _eventBus.Publish(@event);
            
            return Task.CompletedTask;
        }
    }
}