using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public class EventPublisher : IEventPublisher
    {
        private readonly ILogger<EventPublisher> _logger;
        private readonly IOutboxListener _listener;
        private readonly IEventStore _eventStore;

        public EventPublisher(ILogger<EventPublisher> logger,
            IOutboxListener listener, IEventStore eventStore)
        {
            _logger = logger;
            _listener = listener;
            _eventStore = eventStore;
        }

        public Task PublishAsync(IntegrationEvent @event) => PublishCoreAsync(@event);

        public async Task PublishAsync(IEnumerable<IntegrationEvent> events)
        {
            foreach (var @event in events)
            {
                await PublishCoreAsync(@event);
            }
        }

        private async Task PublishCoreAsync(IntegrationEvent @event)
        {
            try
            {
                await _eventStore.AddAsync(@event);
                _listener.OnNewMessage(@event.Id.ToString());
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error publishing event id: {EventId}, {@IntegrationEvent}", @event.Id, @event);
            }
        }
    }
}