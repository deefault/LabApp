using System.Collections.Generic;
using LabApp.Shared.EventConsistency.Abstractions;

// ReSharper disable once CheckNamespace
namespace LabApp.Shared.EventConsistency
{
    public abstract class EventEntity
    {
        public IEnumerable<BaseIntegrationEvent> Events { get; private set; }

        public void AddEvent(BaseIntegrationEvent @event)
        {
            if (Events == null)
            {
                Events = new List<BaseIntegrationEvent> {@event};
            }
            else
            {
                ((List<BaseIntegrationEvent>) Events).Add(@event);
            }
        }
    }
}