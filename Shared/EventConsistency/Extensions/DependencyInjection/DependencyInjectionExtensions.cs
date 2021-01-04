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
        /// <returns></returns>
        public static IServiceCollection AddEventConsistency<TIncomingHandler, TOutgoingHandler>(
            this IServiceCollection services)
            where TIncomingHandler : class, IIncomingIntegrationEventHandler
            where TOutgoingHandler : class, IOutgoingIntegrationEventHandler
        {
            services.AddSingleton<IInboxListener, InboxListener>();
            services.AddSingleton<IOutboxListener, OutboxListener>();
            services.AddScoped(typeof(TIncomingHandler));
            services.AddScoped(typeof(TOutgoingHandler));

            services.AddScoped<IInboxEventStore>(); //TODO
            services.AddScoped<IOutboxEventStore>(); //TODO

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