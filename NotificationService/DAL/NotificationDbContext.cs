using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NotificationService.DAL.Models;

namespace NotificationService.DAL
{
    public class NotificationDbContext : DbContext
    {
        private static readonly JsonSerializerSettings _optionsSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        public virtual DbSet<UserNotificationOptions> UserOptions { get; }

        public NotificationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserNotificationOptions>(o =>
            {
                o.HasIndex(x => x.UserId);
                o.Property(x => x.Options)
                    .HasColumnType("json")
                    .HasConversion(
                        x => JsonConvert.SerializeObject(x, _optionsSerializerSettings),
                        x => JsonConvert.DeserializeObject<NotificationOptions>(x, _optionsSerializerSettings));
            });
        }
    }
}