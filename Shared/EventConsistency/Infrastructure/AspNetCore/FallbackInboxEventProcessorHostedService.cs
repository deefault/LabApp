using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    // maybe replaced by quartz.net, hangfire etc
    public class FallbackInboxEventProcessorHostedService : BackgroundFallbackWorkerBase
    {
        public FallbackInboxEventProcessorHostedService(ILogger<BackgroundFallbackWorkerBase> logger,
            IServiceScopeFactory scopeFactory) : base(logger, scopeFactory)
        {
        }

        protected override int TimerInterval => 5;

        protected override async Task DoWorkInternalAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            IFallbackEventProcessor processor = scope.ServiceProvider.GetRequiredService<IFallbackEventProcessor>();
            
            await processor.ProcessAsync();
        }
    }
}