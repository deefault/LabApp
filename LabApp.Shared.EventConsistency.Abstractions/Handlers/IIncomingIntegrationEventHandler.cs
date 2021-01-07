using System;
using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    // internal handler
    public interface IIncomingIntegrationEventHandler
    {
        Task HandleAsync(BaseIntegrationEvent @event, Type eventType, string eventName);
    }

    public interface IIncomingIntegrationEventHandler<TBaseEvent> where TBaseEvent : BaseIntegrationEvent, new()
    {
        Task HandleAsync(TBaseEvent @event, Type eventType, string eventName);
    }
}