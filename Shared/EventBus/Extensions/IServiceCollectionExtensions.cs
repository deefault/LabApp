using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventBus.Interfaces;
using LabApp.Shared.EventBus.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Models;


namespace LabApp.Shared.EventBus.Extensions
{
    public static class IServiceCollectionExtensions
    {
        private static readonly MethodInfo SubscribeMethod = typeof(IEventBus).GetMethod(nameof(IEventBus.Subscribe));

        private static readonly List<(Type handlerType, Type interfaceType)> _handlers = Assembly.GetEntryAssembly()
            ?.GetTypes().Where(x => x.IsClass && !x.IsAbstract)
            .Select(x => (x, x.GetInterfaces().FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IIntegrationEventHandler<>))))
            .Where(x => x.Item2 != null)
            .ToList();

        private const string SectionName = "Rabbit";

        public static IServiceCollection AddCommon(this IServiceCollection services)
        {
            if (services.Any(x => x.ServiceType == typeof(IEventBus))) return services;
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddRabbitMqClient(configuration.GetSection(SectionName));
            services.AddSingleton<ConnectionFactory>(x =>
            {
                var factory = new ConnectionFactory();

                return factory.ConfigureFromSection(configuration.GetSection(SectionName));
            });
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>(sp =>
                new DefaultRabbitMQPersistentConnection(sp.GetRequiredService<ConnectionFactory>(),
                    sp.GetRequiredService<IConfiguration>(),
                    sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>(),
                    sp.GetRequiredService<RabbitMqConnectionOptionsContainer>()
                ));
            services.AddSingleton<ISubscriptionsManager, SubscriptionsManager>();
            services.AddSingleton<IEventBus, RabbitMQEventBus>();
            services.AddEventHandlers();

            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            // !! Add services before build ServiceProvider
            foreach (var pair in _handlers)
            {
                services.AddScoped(pair.handlerType);
            }
            
            // Subscribe to events
            var sp = services.BuildServiceProvider();
            var eventBusService = sp.GetService<IEventBus>();
            
            foreach (var pair in _handlers)
            {
                Type eventType = pair.interfaceType.GenericTypeArguments.First();
                SubscribeMethod.MakeGenericMethod(eventType, pair.handlerType).Invoke(eventBusService, null);   
            }
            
            return services;
        }

        private static ConnectionFactory ConfigureFromSection(this ConnectionFactory factory,
            IConfigurationSection section)
        {
            // var type = typeof(ConnectionFactory);
            // var properties = type.GetProperties();
            //
            // foreach (PropertyInfo property in properties)
            // {
            //     if (!string.IsNullOrWhiteSpace(section[property.Name]))
            //     {
            //         property.SetValue(factory, section[property.Name]);
            //     }
            // }
            factory.HostName = section["HostName"];
            factory.DispatchConsumersAsync = true;

            if (!string.IsNullOrEmpty(section["UserName"])) factory.UserName = section["UserName"];
            if (!string.IsNullOrEmpty(section["Password"])) factory.Password = section["Password"];
            if (!string.IsNullOrEmpty(section["Port"])) factory.Port = Convert.ToInt32(section["Port"]);

            return factory;
        }
    }
}