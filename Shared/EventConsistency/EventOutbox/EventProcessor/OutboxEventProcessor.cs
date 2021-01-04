using System;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public interface IOutboxEventProcessor : IEventProcessor
    {
    }

    public class OutboxEventProcessor : IOutboxEventProcessor
    {
        private readonly IOutgoingIntegrationEventHandler _handler;
        private readonly IOutboxEventStore _eventStore;
        private readonly ILogger<OutboxEventProcessor> _logger;

        public OutboxEventProcessor(IOutboxEventStore eventStore, ILogger<OutboxEventProcessor> logger,
            IOutgoingIntegrationEventHandler handler)
        {
            _eventStore = eventStore;
            _logger = logger;
            _handler = handler;
        }

        public async Task ProcessEventAsync(string id)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    EventMessage eventMessage = await _eventStore.GetEventAsync(id);

                    if (eventMessage != null && !eventMessage.IsProcessed)
                    {
                        var integrationEvent = eventMessage.EventData;
                        if (await _eventStore.TryDeleteEventAsync(eventMessage))
                        {
                            await _handler.HandleAsync(integrationEvent);
                        }
                    }

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing publishing event id: {EventId}", id);
            }
        }
    }
}