using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IOutgoingIntegrationEventHandler
    {
        Task HandleAsync(IntegrationEvent @event);
    }
    
    /*public interface IOutgoingIntegrationEventHandler<in TBaseEvent> where TBaseEvent: IIntegrationEvent
    {
        Task HandleAsync(TBaseEvent @event);
    }*/
}