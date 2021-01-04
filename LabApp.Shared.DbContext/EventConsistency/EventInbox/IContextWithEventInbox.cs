using LabApp.Shared.EventConsistency;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.DbContext.EventConsistency.EventInbox
{
    public interface IContextWithEventInbox
    {
        DbSet<InboxEventMessage> EventInbox { get; set; } 
    }
}