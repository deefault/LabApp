using System;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventOutbox;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventInbox
{
    public interface IFallbackInboxEventProcessor
    {
        Task ProcessAsync();
    }

    public class FallbackInboxEventProcessor : IFallbackInboxEventProcessor
    {
        private readonly ILogger<FallbackOutboxEventProcessor> _logger;
        private readonly IInboxEventStore _eventStore;
        private readonly IInboxEventProcessor _eventProcessor;

        private const int FallbackDays = 2;

        public FallbackInboxEventProcessor(ILogger<FallbackOutboxEventProcessor> logger,
            IInboxEventStore eventStore, IInboxEventProcessor eventProcessor)
        {
            _logger = logger;
            _eventStore = eventStore;
            _eventProcessor = eventProcessor;
        }

        public async Task ProcessAsync()
        {
            _logger.LogTrace("Starting fallback EventProcessor");

            var messages = (await _eventStore.GetUnpublishedAsync(TimeSpan.FromDays(FallbackDays))).ToArray();
            if (messages.Length == 0) return;

            _logger.LogInformation("Processing {EventCount} events in fallback EventProcessor", messages.Length);

            try
            {
                foreach (var message in messages)
                {
                    await _eventProcessor.ProcessAsync(message.Id);   
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing events in fallback EventProcessor");
            }
        }
    }
}