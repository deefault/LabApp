using System;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventBus.Interfaces
{
	public interface IEventBus : IDisposable
	{
		void Publish(IntegrationEvent @event);

		void Subscribe<T, TH>()
			where T : IntegrationEvent
			where TH : IIntegrationEventHandler<T>;

		void Unsubscribe<T, TH>()
			where TH : IIntegrationEventHandler<T>
			where T : IntegrationEvent;
	}
}