using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nito.AsyncEx;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    public abstract class BackgroundFallbackWorkerBase : BackgroundService
    {
        protected readonly IServiceScopeFactory _scopeFactory;
        protected readonly ILogger<BackgroundFallbackWorkerBase> _logger;

        private Timer _timer;
        private static readonly AsyncLock TimerLock = new AsyncLock();
        private static bool _isRunning = false;

        protected abstract int TimerInterval { get; }
        
        protected BackgroundFallbackWorkerBase(ILogger<BackgroundFallbackWorkerBase> logger,
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
        
        protected abstract Task DoWorkInternalAsync(); 

        public override void Dispose()
        {
            base.Dispose();
            _timer?.Dispose();
        }
    }
}