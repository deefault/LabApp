using System;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventConsistency
{
    public abstract class EventMessage
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? DateDelete { get; set; }
        public IntegrationEvent EventData { get; set; }

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
}