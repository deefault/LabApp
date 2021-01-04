using LabApp.Shared.DbContext.EventConsistency.EventInbox;
using LabApp.Shared.DbContext.EventConsistency.EventOutbox;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.DbContext.EventConsistency
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddEventInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventInboxConfiguration());

            return modelBuilder;
        }
        
        public static ModelBuilder AddEventOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventOutboxConfiguration());

            return modelBuilder;
        }
        
        public static ModelBuilder AddEventConsistency(this ModelBuilder modelBuilder) =>
            modelBuilder
                .AddEventInbox()
                .AddEventOutbox();
    }
}