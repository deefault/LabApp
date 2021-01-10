using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    /// <summary>
    /// internal handler
    /// </summary>
    public interface IInternalOutgoingIntegrationEventHandler
    {
        Task HandleAsync(BaseIntegrationEvent @event);
    }
    
    /// <summary>
    /// External event handler
    /// </summary>
    /// <typeparam name="TBaseEvent"></typeparam>
    /// <example>Publish to event bus</example>
    public interface IOutgoingIntegrationEventHandler<in TBaseEvent> where TBaseEvent : BaseIntegrationEvent, new()
    {
        Task HandleAsync(TBaseEvent @event);
    }
}
