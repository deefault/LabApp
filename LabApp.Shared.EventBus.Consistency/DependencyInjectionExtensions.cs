using System;
using LabApp.Shared.EventBus.Consistency;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Extensions;
using LabApp.Shared.EventConsistency;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEventBusWithConsistency(this IServiceCollection services,
            IConfiguration configuration, Action<EventConsistencyOptions> action = null)
        {
            services.AddEventBus(configuration);
            services.AddEventConsistency<IncomingIntegrationEventHandlerAdapter, OutgoingIntegrationEventHandler, IntegrationEvent>(action);

            return services;
        }
    }
}