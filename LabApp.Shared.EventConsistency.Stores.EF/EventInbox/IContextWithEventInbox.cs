using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF.EventInbox
{
    public interface IContextWithEventInbox : IHasDbContext
    {
        DbSet<InboxEventMessage> EventInbox { get; set; }
    }
}