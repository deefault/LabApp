using System;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventInbox;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public class InternalOutgoingIntegrationEvent<TEvent>
        : IInternalOutgoingIntegrationEventHandler where TEvent : BaseIntegrationEvent, new()
    {
        private readonly IOutgoingIntegrationEventHandler<TEvent> _handler;
        private readonly ILogger<InternalIncomingIntegrationEventHandler<TEvent>> _logger;

        public InternalOutgoingIntegrationEvent(ILogger<InternalIncomingIntegrationEventHandler<TEvent>> logger,
            IOutgoingIntegrationEventHandler<TEvent> handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task HandleAsync(BaseIntegrationEvent @event)
        {
            try
            {
                await _handler.HandleAsync((TEvent) @event);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in HandleAsync for event with id {IntegrationEventId}", @event.Id);
            }
        }
    }
}