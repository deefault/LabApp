using System;
using System.Threading.Tasks;
using System.Transactions;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventInbox
{
    public interface IInboxEventProcessor : IEventProcessor
    {
    }

    public class InboxEventProcessor : IInboxEventProcessor
    {
        private readonly IInboxEventStore _eventStore;
        private readonly IIncomingIntegrationEventHandler _handler;
        private readonly ILogger<InboxEventProcessor> _logger;

        public InboxEventProcessor(IInboxEventStore eventStore, ILogger<InboxEventProcessor> logger,
            IIncomingIntegrationEventHandler handler)
        {
            _eventStore = eventStore;
            _logger = logger;
            _handler = handler;
        }

        public async Task ProcessAsync(string id)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    EventMessage eventMessage = await _eventStore.GetEventAsync(id);

                    if (eventMessage != null && !eventMessage.IsProcessed)
                    {
                        var eventType = eventMessage.EventData.GetType();
                        try
                        {
                            if (await _eventStore.TryDeleteEventAsync(eventMessage))
                            {
                                await _handler.HandleAsync(eventMessage.EventData, eventType, eventType.Name);
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Error handling event handler for event {EventId}", eventMessage.Id);
                            await _eventStore.IncrementFailedAsync((InboxEventMessage)eventMessage);
                        }
                    }

                    transaction.Complete();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing inbox event id: {EventId}", id);
            }
        }
    }
}