using System;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventInbox
{
    public class IntegrationEventInboxHandler
        : IIncomingIntegrationEventHandler
    {
        private readonly IInboxListener _listener;
        private readonly IInboxEventStore _eventStore;
        private readonly ILogger<IntegrationEventInboxHandler> _logger;

        public IntegrationEventInboxHandler(IInboxListener listener, IInboxEventStore eventStore,
            ILogger<IntegrationEventInboxHandler> logger)
        {
            _listener = listener;
            _eventStore = eventStore;
            _logger = logger;
        }

        public async Task HandleAsync(IntegrationEvent @event, Type eventType, string eventName)
        {
            try
            {
                await _eventStore.AddAsync(@event);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Exception in HandleAsync");
            }

            _listener.OnNewMessage(@event.Id.ToString());
        }
    }
}