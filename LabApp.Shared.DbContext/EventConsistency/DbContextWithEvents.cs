using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.DbContext.EventConsistency
{
    public class DbContextWithEvents : Microsoft.EntityFrameworkCore.DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddEventConsistency();
        }
    }
}