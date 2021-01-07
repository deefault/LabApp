using System;
using RabbitMQ.Client;

namespace LabApp.Shared.EventBus.Transports.RabbitMQ
{
	public interface IRabbitMQPersistentConnection
		: IDisposable
	{
		bool IsConnected { get; }

		bool TryConnect();

		IModel CreateModel();
	}
}