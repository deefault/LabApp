using System;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public interface IFallbackOutboxEventProcessor : IFallbackEventProcessor
    {
    }

    public class FallbackOutboxEventProcessor : IFallbackOutboxEventProcessor
    {
        private readonly ILogger<FallbackOutboxEventProcessor> _logger;
        private readonly IOutboxEventStore _eventStore;
        private readonly IOutboxEventProcessor _eventProcessor;

        private const int FallbackDays = 2;

        public FallbackOutboxEventProcessor(ILogger<FallbackOutboxEventProcessor> logger,
            IOutboxEventStore eventStore, IOutboxEventProcessor eventProcessor)
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
                foreach (var integrationEvent in messages)
                {
                    await _eventProcessor.ProcessAsync(integrationEvent.Id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error processing events in fallback EventProcessor");
            }
        }
    }
}