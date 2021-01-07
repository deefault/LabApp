using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventBus.Events.Abstractions
{
    public abstract class IntegrationEventHandlerBase<TEvent> : IIntegrationEventHandler<TEvent>
        where TEvent : IntegrationEvent
    {
        protected readonly ILogger<IntegrationEventHandlerBase<TEvent>> Logger;

        protected IntegrationEventHandlerBase(ILogger<IntegrationEventHandlerBase<TEvent>> logger)
        {
            Logger = logger;
        }

        public virtual Task Handle(TEvent @event)
        {
            Logger.LogDebug("Processing event {IntegrationEventId}", @event.Id);

            return HandleInternal(@event);
        }

        protected abstract Task HandleInternal(TEvent @event);
    }
}