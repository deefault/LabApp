using System;
using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    // internal handler
    public interface IInternalIncomingIntegrationEventHandler
    {
        Task HandleAsync(BaseIntegrationEvent @event, Type eventType, string eventName);
    }

    /// <summary>
    /// external event handler
    /// </summary>
    /// <typeparam name="TBaseEvent"></typeparam>
    /// <example>Find and execute handler for event, sent ack to event bus</example>
    public interface IIncomingIntegrationEventHandler<TBaseEvent> where TBaseEvent : BaseIntegrationEvent, new()
    {
        Task HandleAsync(TBaseEvent @event, Type eventType, string eventName);
    }
}