using System;
using System.Linq;
using LabApp.Shared.EventBus.Interfaces;
using LabApp.Shared.EventBus.Options;
using LabApp.Shared.EventBus.Transports;
using LabApp.Shared.EventBus.Transports.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Models;

namespace LabApp.Shared.EventBus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            if (services.Any(x => x.ServiceType == typeof(IEventBus))) return services;
            services.AddScoped<IIncomingEventHandler, IncomingEventHandler>();
            services.AddConfigurations(configuration);
            
            services.AddRabbitMqClient(configuration.GetSection(RabbitMqOptions.Key));
            services.AddSingleton<ConnectionFactory>(x =>
            {
                var factory = new ConnectionFactory();

                return factory.ConfigureFromSection(configuration.GetSection(RabbitMqOptions.Key));
            });
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>(sp =>
                new DefaultRabbitMQPersistentConnection(sp.GetRequiredService<ConnectionFactory>(),
                    sp.GetRequiredService<IConfiguration>(),
                    sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>(),
                    sp.GetRequiredService<RabbitMqConnectionOptionsContainer>(),
                    sp.GetRequiredService<IHostApplicationLifetime>()
                ));
            services.AddSingleton<ISubscriptionsManager, SubscriptionsManager>();
            services.AddSingleton<IEventBus, RabbitMQEventBus>();
            services.AddEventHandlers();
            services.AddHostedService<EventBusSubscriptionService>();

            return services;
        }

        private static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            // !! Add services before build ServiceProvider
            foreach (var pair in SubscriptionsManager.HandlersInAssembly)
            {
                services.AddTransient(pair.handlerType);
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