using LabApp.Shared.EventBus.Events.Abstractions;
using LabApp.Shared.EventConsistency;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LabApp.Shared.DbContext.EventConsistency.EventOutbox
{
    public class EventOutboxConfiguration : IEntityTypeConfiguration<OutboxEventMessage>
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
        
        public void Configure(EntityTypeBuilder<OutboxEventMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.DateTime)
                .HasSortOrder(SortOrder.Descending)
                .HasFilter("DateDelete IS NOT NULL");
            builder.Ignore(x => x.IsProcessed);
            builder.Property(x => x.EventData)
                .HasColumnType("json")
                .HasConversion(x => JsonConvert.SerializeObject(x, Settings),
                    x => JsonConvert.DeserializeObject<IntegrationEvent>(x, Settings));
        }
    }
}