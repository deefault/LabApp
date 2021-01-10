using System;
using System.Linq;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventInbox;
using LabApp.Shared.EventConsistency.EventOutbox;
using LabApp.Shared.EventConsistency.Infrastructure.AspNetCore;
using LabApp.Shared.EventConsistency.Stores.EF;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class DependencyInjectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TIncomingHandler"></typeparam>
        /// <typeparam name="TOutgoingHandler"></typeparam>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddEventConsistency<TIncomingHandler, TOutgoingHandler, TEvent>(
            this IServiceCollection services, Action<EventConsistencyOptions> configuringAction = null)
            where TIncomingHandler : class, IIncomingIntegrationEventHandler<TEvent>
            where TOutgoingHandler : class, IOutgoingIntegrationEventHandler<TEvent>
            where TEvent : BaseIntegrationEvent, new()
        {
            services.AddOptions<EventConsistencyOptions>(EventConsistencyOptions.Key);

            services.Configure(configuringAction ?? EventConsistencyOptions.DefaultConfiguration);

            if (services.FirstOrDefault(x => x.ServiceType == typeof(IInboxEventStore)) == null ||
                services.FirstOrDefault(x => x.ServiceType == typeof(IOutboxEventStore)) == null)
            {
                services.AddDefaultEventStores();
                //throw new Exception("Please, register IInboxEventStore, IOutboxEventStore types");
            }

            services.AddSingleton<IInboxListener, InboxListener>();
            services.AddSingleton<IOutboxListener, OutboxListener>();

            services.AddScoped<IInboxEventProcessor, InboxEventProcessor>();
            services.AddScoped<IOutboxEventProcessor, OutboxEventProcessor>();

            services.AddScoped(typeof(IIncomingIntegrationEventHandler<TEvent>), typeof(TIncomingHandler));
            services.AddScoped(typeof(IOutgoingIntegrationEventHandler<TEvent>), typeof(TOutgoingHandler));
            services
                .AddScoped<IInternalIncomingIntegrationEventHandler, InternalIncomingIntegrationEventHandler<TEvent>>();
            services.AddScoped<IInternalOutgoingIntegrationEventHandler, InternalOutgoingIntegrationEvent<TEvent>>();

            services.AddScoped<IFallbackInboxEventProcessor, FallbackInboxEventProcessor>();
            services.AddScoped<IFallbackOutboxEventProcessor, FallbackOutboxEventProcessor>();
            services.AddHostedService<FallbackInboxEventProcessorHostedService>();
            services.AddHostedService<FallbackOutboxEventProcessorHostedService>();

            return services;
        }

        public static IServiceCollection AddDefaultEventStores(this IServiceCollection services)
        {
            return services.Any(x => x.ServiceType == typeof(DbContextWithEvents))
                ? services
                : services.AddEventStore();
        }
    }
}