using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventInbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    // maybe replaced by quartz.net, hangfire etc
    public class FallbackInboxEventProcessorHostedService : BackgroundFallbackWorkerBase
    {

        public FallbackInboxEventProcessorHostedService(ILogger<BackgroundFallbackWorkerBase> logger,
            IServiceScopeFactory scopeFactory, IOptions<EventConsistencyOptions> options) : base(logger, scopeFactory)
        {
            Enabled = options.Value.EnableInbox;
        }

        protected override int TimerInterval => 5;

        protected override async Task DoWorkInternalAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            IFallbackInboxEventProcessor processor = scope.ServiceProvider.GetRequiredService<IFallbackInboxEventProcessor>();
            
            await processor.ProcessAsync();
        }
    }
}