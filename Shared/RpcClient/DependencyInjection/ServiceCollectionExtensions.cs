using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LabApp.Shared.RpcClient.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMQRpcClient(this IServiceCollection services)
        {
            services.AddOptions<RpcClientOptions>().Configure(x => new RpcClientOptions());
            services.TryAddScoped<IRpcClient, RabbitMQRpcClient>();
            services.TryAddScoped<IRpcClient, RabbitMQRpcClient>();

            return services;
        }
    }
}