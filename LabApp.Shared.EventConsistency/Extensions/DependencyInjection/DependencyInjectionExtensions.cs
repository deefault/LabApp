using System;
using System.Linq;
using LabApp.Shared.EventConsistency;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventInbox;
using LabApp.Shared.EventConsistency.EventOutbox;
using LabApp.Shared.EventConsistency.Infrastructure.AspNetCore;
using Microsoft.Extensions.Logging;

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
                throw new Exception("Please, register IInboxEventStore, IOutboxEventStore types");
            }

            services.AddSingleton<IInboxListener, InboxListener>();
            services.AddSingleton<IOutboxListener, OutboxListener>();
            services.AddScoped(typeof(IIncomingIntegrationEventHandler<TEvent>), typeof(TIncomingHandler));
            services.AddScoped(typeof(IOutgoingIntegrationEventHandler<TEvent>), typeof(TOutgoingHandler));//TODO:!!!!

            services.AddScoped<IInboxEventProcessor, InboxEventProcessor>(sp =>
                new InboxEventProcessor(sp.GetRequiredService<IInboxEventStore>(),
                    sp.GetRequiredService<ILogger<InboxEventProcessor>>(),
                    (IIncomingIntegrationEventHandler) sp.GetRequiredService(typeof(TIncomingHandler))));
            services.AddScoped<IOutboxEventProcessor, OutboxEventProcessor>(sp =>
                new OutboxEventProcessor(sp.GetRequiredService<IOutboxEventStore>(),
                    sp.GetRequiredService<ILogger<OutboxEventProcessor>>(),
                    (IOutgoingIntegrationEventHandler) sp.GetRequiredService(typeof(TOutgoingHandler))));

            services.AddScoped<IOutboxEventProcessor, OutboxEventProcessor>();
            services.AddScoped<IFallbackInboxEventProcessor, FallbackInboxEventProcessor>();
            services.AddScoped<IFallbackOutboxEventProcessor, FallbackOutboxEventProcessor>();
            services.AddHostedService<FallbackInboxEventProcessorHostedService>();
            services.AddHostedService<FallbackOutboxEventProcessorHostedService>();

            services.AddScoped<IIncomingIntegrationEventHandler, IntegrationEventInboxHandler>();

            return services;
        }
    }
}