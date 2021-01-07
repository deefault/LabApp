using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IOutgoingIntegrationEventHandler
    {
        Task HandleAsync(BaseIntegrationEvent @event);
    }

    public interface IOutgoingIntegrationEventHandler<in TBaseEvent> where TBaseEvent : BaseIntegrationEvent, new()
    {
        Task HandleAsync(TBaseEvent @event);
    }
}
