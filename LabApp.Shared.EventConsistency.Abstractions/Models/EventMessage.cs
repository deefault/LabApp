using System;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public abstract class EventMessage
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? DateDelete { get; set; }
        public BaseIntegrationEvent EventData { get; set; }

        public bool IsProcessed => DateDelete != null;
    }

    public class InboxEventMessage : EventMessage
    {
        public int FailedProcessingCount { get; set; }
    }
    
    public class OutboxEventMessage : EventMessage
    {
        public int FailedProcessingCount { get; set; }
    }

    public static class EventMessageExtensions
    {
        public static OutboxEventMessage ToOutboxEventMessage(this BaseIntegrationEvent @event)
        {
            return new OutboxEventMessage()
            {
                Id = @event.Id.ToString(),
                DateTime = DateTime.UtcNow,
                EventData = @event
            };
        }
        
        public static InboxEventMessage ToInboxEventMessage(this BaseIntegrationEvent @event)
        {
            return new InboxEventMessage()
            {
                Id = @event.Id.ToString(),
                DateTime = DateTime.UtcNow,
                EventData = @event
            };
        }
    }
}