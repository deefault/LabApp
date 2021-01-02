using System;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.DbContext.Models
{
    public class EventMessage
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime? DateDelete { get; set; }
        public IntegrationEvent EventData { get; set; }

        public bool IsDeleted => DateDelete != null;
    }
}