using LabApp.Shared.DbContext.Models;
using LabApp.Shared.EventBus.Events.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LabApp.Shared.DbContext.EventOutbox
{
    public class EventOutboxConfiguration : IEntityTypeConfiguration<EventMessage>
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
        
        public void Configure(EntityTypeBuilder<EventMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.DateTime)
                .HasSortOrder(SortOrder.Descending)
                .HasFilter("DateDelete IS NOT NULL");
            builder.Ignore(x => x.IsDeleted);
            builder.Property(x => x.EventData)
                .HasColumnType("json")
                .HasConversion(x => JsonConvert.SerializeObject(x, Settings),
                    x => JsonConvert.DeserializeObject<IntegrationEvent>(x, Settings));
        }
    }
}