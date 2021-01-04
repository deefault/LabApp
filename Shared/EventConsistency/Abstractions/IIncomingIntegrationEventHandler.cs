using System;
using System.Threading.Tasks;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    // internal handler
    public interface IIncomingIntegrationEventHandler
    {
        Task HandleAsync(IntegrationEvent @event, Type eventType, string eventName);
    }
    
    /*public interface IIncomingIntegrationEventHandler<TBaseEvent> where TBaseEvent: IIntegrationEvent 
    {
        Task HandleAsync(TBaseEvent @event, Type eventType, string eventName);
    }*/
}