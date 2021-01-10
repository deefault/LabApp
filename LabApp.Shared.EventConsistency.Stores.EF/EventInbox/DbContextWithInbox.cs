using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF.EventInbox
{
    public abstract class DbContextWithInbox : DbContext, IContextWithEventInbox
    {
        public DbSet<InboxEventMessage> EventInbox { get; set; }
        public DbContext Context => this;
    }
}