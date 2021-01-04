using LabApp.Shared.DbContext.EventConsistency;
using LabApp.Shared.DbContext.EventConsistency.EventOutbox;
using LabApp.Shared.EventConsistency;
using Microsoft.EntityFrameworkCore;

namespace LabApp.DbContext.Tests.EventOutboxDbContextTests
{
    public class FakeDbContext : Microsoft.EntityFrameworkCore.DbContext, IContextWithEventOutbox
    {
        public virtual DbSet<FakeEntity> Entities { get; set; }
        public DbSet<OutboxEventMessage> EventOutbox { get; set; }

        public FakeDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OutboxEventMessage>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<FakeEntity>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<FakeEvent>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.AddEventOutbox();
        }
    }
}