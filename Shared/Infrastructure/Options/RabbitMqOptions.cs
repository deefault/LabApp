using RabbitMQ.Client.Core.DependencyInjection.Configuration;

namespace LabApp.Shared.Infrastructure.Options
{
    public class RabbitMqOptions : RabbitMqClientOptions
    {
        public const string Key = "Rabbit";
        
        public string BrokerName { get; set; }
        public string QueueName { get; set; }
        public string Username => UserName;
        public new string Password { get; set; }
        public new string HostName { get; set; }
        public new int Port { get; set; }
        public bool DispatchConsumersAsync { get; set; }
    }
}