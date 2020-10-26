using LabApp.Shared.Infrastructure.Options;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.Key));
            
            return serviceCollection;
        }
    }
}