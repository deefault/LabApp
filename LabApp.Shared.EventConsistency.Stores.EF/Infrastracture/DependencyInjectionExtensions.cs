using System;
using System.Linq;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.Stores.EF.EventInbox;
using LabApp.Shared.EventConsistency.Stores.EF.EventOutbox;
using Microsoft.Extensions.DependencyInjection;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEventStore<TContext>(this IServiceCollection services)
            where TContext : DbContextWithEvents =>
            services.AddEventStore(typeof(TContext), typeof(TContext));


        public static IServiceCollection AddEventStore(this IServiceCollection services)
        {
            var outboxContextDescriptor =
                services.FirstOrDefault(x => x.ServiceType.IsClass &&
                                             (x.ServiceType.BaseType == typeof(DbContextWithEvents) ||
                                              x.ServiceType.GetInterfaces().Contains(typeof(IContextWithEventOutbox))));
            var inboxContextDescriptor =
                services.FirstOrDefault(x => x.ServiceType.IsClass &&
                                             (x.ServiceType.BaseType == typeof(DbContextWithEvents) ||
                                              x.ServiceType.GetInterfaces().Contains(typeof(IContextWithEventInbox))));

            services.PostConfigure((EventConsistencyOptions x) =>
            {
                bool isInboxContextRegistered = inboxContextDescriptor != null;
                bool isOutboxContextRegistered = outboxContextDescriptor != null;
                if (x.EnableInbox != isInboxContextRegistered && x.EnableInbox)
                {
                    //TODO: warning or throw
                }

                if (x.EnableOutbox != isOutboxContextRegistered && x.EnableOutbox)
                {
                    //TODO: warning or throw
                }
                
                x.EnableInbox = isInboxContextRegistered;
                x.EnableOutbox = isOutboxContextRegistered;
                x.InboxStoreType = StoreType.Ef;
                x.OutboxStoreType = StoreType.Ef;
            });

            return services.AddEventStore(inboxContextDescriptor?.ServiceType, outboxContextDescriptor?.ServiceType);
        }

        internal static IServiceCollection AddEventStore(this IServiceCollection services, Type inboxContextType,
            Type outboxContextType)
        {
            if (inboxContextType != null)
            {
                services.AddScoped(typeof(IContextWithEventInbox), inboxContextType);
                services.AddCustomInboxStore<EfInboxEventStore>();
            }
            else
            {
                services.AddCustomInboxStore<NullInboxEventStore>();
            }

            if (outboxContextType != null)
            {
                services.AddScoped(typeof(IContextWithEventOutbox), outboxContextType);
                services.AddCustomOutboxStore<EfOutboxEventStore>();
            }
            else
            {
                services.AddCustomOutboxStore<NullOutboxEventStore>();
            }

            return services;
        }
        
        public static IServiceCollection AddCustomInboxStore<TInbox>(this IServiceCollection services)
            where TInbox : class, IInboxEventStore
        {
            services.AddScoped(typeof(IInboxEventStore), typeof(TInbox));

            return services;
        }
        
        public static IServiceCollection AddCustomOutboxStore<TOutbox>(this IServiceCollection services)
            where TOutbox : class, IOutboxEventStore
        {
            services.AddScoped(typeof(IOutboxEventStore), typeof(TOutbox));

            return services;
        }
    }
}