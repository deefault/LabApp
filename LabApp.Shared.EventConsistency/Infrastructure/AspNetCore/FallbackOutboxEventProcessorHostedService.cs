using System;
using System.Threading;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.EventInbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    // maybe replaced by quartz.net, hangfire etc
    public class FallbackOutboxEventProcessorHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<FallbackOutboxEventProcessorHostedService> _logger;

        private Timer _timer;
        private static readonly AsyncLock TimerLock = new AsyncLock();
        private static bool _isRunning = false;
        private const int TimerInterval = 5; // TODO: move to configuration

        public FallbackOutboxEventProcessorHostedService(ILogger<FallbackOutboxEventProcessorHostedService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundEventProcessor initializing");
            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero, TimeSpan.FromSeconds(TimerInterval));
            
            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object state)
        {
            if (_isRunning) return;

            using (var l = await TimerLock.LockAsync())
            {
                _isRunning = true;
                try
                {
                    await DoWorkInternalAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error in FallbackEventProcessorHostedService");
                }
                finally
                {
                    _isRunning = false;
                }
            }
        }

        private async Task DoWorkInternalAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            IFallbackInboxEventProcessor processor = scope.ServiceProvider.GetRequiredService<IFallbackInboxEventProcessor>();
            
            await processor.ProcessAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }
}