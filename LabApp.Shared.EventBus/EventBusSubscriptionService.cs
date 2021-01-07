using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventBus
{
    public class EventBusSubscriptionService : IHostedService
    {
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IServiceProvider _sp;
        private readonly ILogger<EventBusSubscriptionService> _logger;
        
        private static readonly MethodInfo SubscribeMethod = typeof(IEventBus).GetMethod(nameof(IEventBus.Subscribe));

        public EventBusSubscriptionService(IHostApplicationLifetime lifetime, IServiceProvider sp,
            ILogger<EventBusSubscriptionService> logger)
        {
            _lifetime = lifetime;
            _sp = sp;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Registering ApplicationStarted callback..");
            _lifetime.ApplicationStarted.Register(() =>
            {
                _logger.LogInformation("ApplicationStarted callback called");
                var eventBusService = _sp.GetService<IEventBus>();
                foreach ((Type handlerType, Type interfaceType) in SubscriptionsManager.HandlersInAssembly)
                {
                    Type eventType = interfaceType.GenericTypeArguments.First();
                    SubscribeMethod.MakeGenericMethod(eventType, handlerType).Invoke(eventBusService, null);   
                }
            });
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("StopAsync called in EventBusSubscriptionService");
            _sp.GetRequiredService<IEventBus>().Dispose();
            
            return Task.CompletedTask;
        }
    }
}