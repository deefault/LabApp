using LabApp.Shared.EventConsistency.Stores.EF.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddEventInbox(this ModelBuilder modelBuilder, DatabaseFacade databaseFacade)
        {
            modelBuilder.ApplyConfiguration(new EventInboxConfiguration(databaseFacade));

            return modelBuilder;
        }
        
        public static ModelBuilder AddEventOutbox(this ModelBuilder modelBuilder, DatabaseFacade databaseFacade)
        {
            modelBuilder.ApplyConfiguration(new EventOutboxConfiguration(databaseFacade));

            return modelBuilder;
        }
        
        public static ModelBuilder AddEventConsistency(this ModelBuilder modelBuilder, DatabaseFacade databaseFacade) =>
            modelBuilder
                .AddEventInbox(databaseFacade)
                .AddEventOutbox(databaseFacade);
    }
}