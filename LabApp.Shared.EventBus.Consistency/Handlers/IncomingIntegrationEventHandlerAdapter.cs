using System;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Transports;
using LabApp.Shared.EventConsistency.Abstractions;

namespace LabApp.Shared.EventBus.Consistency
{
    public class IncomingIntegrationEventHandlerAdapter 
        : IIncomingIntegrationEventHandler<IntegrationEvent>
    {
        private readonly IIncomingEventHandler _handler;

        public IncomingIntegrationEventHandlerAdapter(IIncomingEventHandler handler)
        {
            _handler = handler;
        }

        public Task HandleAsync(IntegrationEvent @event, Type eventType, string eventName)
        {
            return _handler.HandleAsync(@event, eventType, eventName);
        }
    }
}