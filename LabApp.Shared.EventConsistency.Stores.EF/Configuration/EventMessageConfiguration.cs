using System;
using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LabApp.Shared.EventConsistency.Stores.EF.Configuration
{
    public class EventMessageConfiguration
    {
        protected readonly DatabaseFacade Database;

        public EventMessageConfiguration(DatabaseFacade database)
        {
            Database = database;
        }
        
        protected static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
        
        protected string ResolveColumnType()
        {
            if (Database.IsNpgsql())
            {
                return "json";
            }

            if (Database.IsSqlServer())
            {
                return "text";
            }

            if (!Database.IsRelational())
            {
                return "json";
            }
            
            throw new NotSupportedException();
        }
        
    }
    
    
    public class EventOutboxConfiguration : EventMessageConfiguration, IEntityTypeConfiguration<OutboxEventMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxEventMessage> builder)
        {
            builder.HasKey(x => x.Id);
            var indexBuilder = builder.HasIndex(x => x.DateTime);
            if (Database.IsNpgsql())
            {
                indexBuilder.HasSortOrder(SortOrder.Descending).HasFilter("DateDelete IS NOT NULL");
            }
            else if (Database.IsSqlServer())
            {
               // TODO
            }
            
            builder.Ignore(x => x.IsProcessed);
            builder.Property(x => x.EventData)
                .HasColumnType(ResolveColumnType())
                .HasConversion(x => JsonConvert.SerializeObject(x, Settings),
                    x => JsonConvert.DeserializeObject<BaseIntegrationEvent>(x, Settings));
        }

        public EventOutboxConfiguration(DatabaseFacade database) : base(database)
        {
        }
    }
    
    public class EventInboxConfiguration : EventMessageConfiguration, IEntityTypeConfiguration<InboxEventMessage>
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };
        
        public void Configure(EntityTypeBuilder<InboxEventMessage> builder)
        {
            builder.HasKey(x => x.Id);
            var indexBuilder = builder.HasIndex(x => x.DateTime);
            if (Database.IsNpgsql())
            {
                indexBuilder.HasSortOrder(SortOrder.Descending).HasFilter("DateDelete IS NOT NULL");
            }
            else if (Database.IsSqlServer())
            {
                // TODO
            }
            
            builder.Ignore(x => x.IsProcessed);
            builder.Property(x => x.EventData)
                .HasColumnType(ResolveColumnType())
                .HasConversion(x => JsonConvert.SerializeObject(x, Settings),
                    x => JsonConvert.DeserializeObject<BaseIntegrationEvent>(x, Settings));
        }

        public EventInboxConfiguration(DatabaseFacade database) : base(database)
        {
        }
    }
}