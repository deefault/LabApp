using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventOutbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    // maybe replaced by quartz.net, hangfire etc
    public class FallbackOutboxEventProcessorHostedService : BackgroundFallbackWorkerBase
    {
        protected override int TimerInterval => 5;
        
        public FallbackOutboxEventProcessorHostedService(ILogger<FallbackOutboxEventProcessorHostedService> logger,
            IServiceScopeFactory scopeFactory, IOptions<EventConsistencyOptions> options) : base(logger, scopeFactory)
        {
            Enabled = options.Value.EnableOutbox;
        }
        

        protected override async Task DoWorkInternalAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            IFallbackOutboxEventProcessor processor = scope.ServiceProvider.GetRequiredService<IFallbackOutboxEventProcessor>();
            
            await processor.ProcessAsync();
        }
    }
}