using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.Stores.EF.EventInbox;
using LabApp.Shared.EventConsistency.Stores.EF.EventOutbox;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    public abstract class DbContextWithEvents : DbContextWithOutbox, IContextWithEventInbox
    {
        public DbContext Context => this;
        public abstract DbSet<InboxEventMessage> EventInbox { get; set; }
        
        protected DbContextWithEvents(DbContextOptions options, IOutboxListener listener) : base(options, listener)
        {
        }
    }
}