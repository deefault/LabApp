using System.Collections.Generic;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.Data
{
    public abstract class EventEntity
    {
        public IEnumerable<IntegrationEvent> Events { get; private set; }

        public void AddEvent(IntegrationEvent @event)
        {
            if (Events == null)
            {
                Events = new List<IntegrationEvent> {@event};
            }
            else
            {
                ((List<IntegrationEvent>) Events).Add(@event);
            }
        }
    }
}