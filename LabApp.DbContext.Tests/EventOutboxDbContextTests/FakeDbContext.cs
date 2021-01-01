using LabApp.Server.Data.EventOutbox;
using LabApp.Shared.Data.EF.EventOutbox;
using Microsoft.EntityFrameworkCore;

namespace LabApp.DbContext.Tests.EventOutboxDbContextTests
{
    public class FakeDbContext : Microsoft.EntityFrameworkCore.DbContext, IContextWithEventOutbox
    {
        public virtual DbSet<FakeEntity> Entities { get; set; }
        public DbSet<EventMessage> EventOutbox { get; set; }

        public FakeDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<FakeEvent>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.ApplyConfiguration(new EventOutboxConfiguration());
        }
    }
}