using System;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IIntegrationEvent
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
    }
}