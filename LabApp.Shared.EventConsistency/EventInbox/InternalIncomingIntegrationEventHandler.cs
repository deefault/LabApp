using System;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventInbox
{
    public class InternalIncomingIntegrationEventHandler<TEvent>
        : IInternalIncomingIntegrationEventHandler where TEvent : BaseIntegrationEvent, new()
    {
        private readonly IIncomingIntegrationEventHandler<TEvent> _handler;
        private readonly ILogger<InternalIncomingIntegrationEventHandler<TEvent>> _logger;

        public InternalIncomingIntegrationEventHandler(ILogger<InternalIncomingIntegrationEventHandler<TEvent>> logger,
            IIncomingIntegrationEventHandler<TEvent> handler)
        {
            _logger = logger;
            _handler = handler;
        }

        public async Task HandleAsync(BaseIntegrationEvent @event, Type eventType, string eventName)
        {
            try
            {
                await _handler.HandleAsync((TEvent) @event, eventType, eventName);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in HandleAsync");
            }
        }
    }
}