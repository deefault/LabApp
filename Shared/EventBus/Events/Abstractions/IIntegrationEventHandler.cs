using System.Threading.Tasks;

namespace LabApp.Shared.EventBus.Events.Abstractions
{
	public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler 
		where TIntegrationEvent: IntegrationEvent
	{
		Task Handle(TIntegrationEvent @event);
	}

	public interface IIntegrationEventHandler
	{
	}
}