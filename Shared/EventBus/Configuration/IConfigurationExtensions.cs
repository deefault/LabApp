using System;
using System.Linq;
using LabApp.Shared.EventBus.Configuration;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace LabApp.Shared.EventBus.ConfigurationExtensions
{
	public static class IConfigurationExtensions
	{ 
		public static string GetBrokerName(this IConfiguration configuration) =>
			configuration.GetSection(Keys.RabbitMq).GetChildren()
				.FirstOrDefault(x => x.Key == "BrokerName")
				?.Value ?? throw new ArgumentException("BrokerName not found");
		
		public static string GetQueueName(this IConfiguration configuration) =>
			configuration.GetSection(Keys.RabbitMq).GetChildren()
				.FirstOrDefault(x => x.Key == "QueueName")
				?.Value ?? throw new ArgumentException("No key QueueName");
	}
}