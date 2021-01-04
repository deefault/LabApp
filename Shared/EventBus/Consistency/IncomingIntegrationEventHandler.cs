using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventConsistency.Abstractions;

namespace LabApp.Shared.EventBus
{
    public class IncomingIntegrationEventHandler : IIncomingIntegrationEventHandler
    {
        private readonly ISubscriptionsManager _subscriptionsManager;
        private readonly IServiceProvider _serviceProvider;

        public IncomingIntegrationEventHandler(ISubscriptionsManager subscriptionsManager,
            IServiceProvider serviceProvider)
        {
            _subscriptionsManager = subscriptionsManager;
            _serviceProvider = serviceProvider;
        }

        public async Task HandleAsync(IntegrationEvent @event, Type eventType, string eventName)
        {
            IEnumerable<Type> eventHandlers = _subscriptionsManager.GetHandlersForEvent(eventName);

            foreach (Type handlerType in eventHandlers)
            {
                var handler = _serviceProvider.GetService(handlerType);
                if (handler == null) continue;

                await ExecuteHandler(eventType, handler, @event);
            }
        }
        
        private static async Task ExecuteHandler(Type eventType, object handler, IntegrationEvent integrationEvent)
        {
            Type concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

            await Task.Yield();
            // ReSharper disable once PossibleNullReferenceException
            await (Task) concreteType.GetMethod("Handle")
                .Invoke(handler, new object[] {integrationEvent});
        }
    }
}