using LabApp.Shared.EventConsistency;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.DbContext.EventConsistency.EventOutbox
{
    public interface IContextWithEventOutbox
    {
        DbSet<OutboxEventMessage> EventOutbox { get; set; } 
    }
}