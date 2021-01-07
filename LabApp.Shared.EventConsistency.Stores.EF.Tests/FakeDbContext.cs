using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.Stores.EF.Tests
{
    public class FakeDbContext : DbContextWithEvents
    {
        public virtual DbSet<FakeEntity> Entities { get; set; }
        public override DbSet<OutboxEventMessage> EventOutbox { get; set; }
        public override DbSet<InboxEventMessage> EventInbox { get; set; }

        public FakeDbContext(DbContextOptions options, IOutboxListener listener) : base(options, listener)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(x =>
            {
                x.Ignore(new EventId(CoreEventId.ProviderBaseId));
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutboxEventMessage>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<FakeEntity>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<FakeEvent>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.AddEventOutbox(Database);
        }
    }
}