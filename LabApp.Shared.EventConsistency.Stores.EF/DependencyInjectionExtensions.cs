using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.Stores.EF.EventInbox;
using LabApp.Shared.EventConsistency.Stores.EF.EventOutbox;
using Microsoft.Extensions.DependencyInjection;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    public static partial class DependencyInjectionExtensions
    {
        public static IServiceCollection AddEventStore<TContext>(this IServiceCollection services)
            where TContext: Microsoft.EntityFrameworkCore.DbContext
        {
            services.AddScoped(typeof(DbContextWithEvents), typeof(TContext));
            // Add default implementation
            services.AddCustomEventStore<EfInboxEventStore, EfOutboxEventStore>();

            return services;
        }
        
        /// <summary>
        /// Add custom inbox and outbox event-store implementation
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TInbox"></typeparam>
        /// <typeparam name="TOutbox"></typeparam>
        /// <returns></returns>
        public static IServiceCollection AddCustomEventStore<TInbox, TOutbox>(this IServiceCollection services)
            where TInbox: class, IInboxEventStore
            where TOutbox: class, IOutboxEventStore
        {
            services.AddScoped(typeof(IInboxEventStore), typeof(TInbox));
            services.AddScoped(typeof(IOutboxEventStore), typeof(TOutbox));

            return services;
        }
    }
}