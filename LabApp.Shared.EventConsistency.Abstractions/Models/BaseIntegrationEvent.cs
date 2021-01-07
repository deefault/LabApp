using System;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public abstract class BaseIntegrationEvent
    {
        public BaseIntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
        
        public BaseIntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }
        
        public Guid Id { get; protected set; }
        public DateTime CreationDate { get; protected set; }
    }
}