using System;
using System.Linq;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.Stores.EF.EventInbox;
using LabApp.Shared.EventConsistency.Stores.EF.EventOutbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    // ReSharper disable once InconsistentNaming
    public class EventConsistencyMigratorEF : IEventConsistencyMigrator
    {
        private const string InboxMigrationName = "EVENT_CONSISTENCY_INBOX_MIGRATION";
        private const string OutboxMigrationName = "EVENT_CONSISTENCY_OUTBOX_MIGRATION";

        private readonly EventConsistencyOptions _options;
        private readonly IServiceProvider _sp;

        public EventConsistencyMigratorEF(IOptions<EventConsistencyOptions> options, IServiceProvider sp)
        {
            _sp = sp;
            _options = options.Value;
        }

        public async Task MigrateAsync()
        {
            if (!_options.EnableInbox && !_options.EnableOutbox) return;

            if (_options.EnableInbox)
            {
                IContextWithEventInbox context = _sp.GetRequiredService<IContextWithEventInbox>();
                if ((await context.Context.Database.GetAppliedMigrationsAsync()).All(x => x != InboxMigrationName))
                {
                    await MigrateInboxAsync(context);
                }
            }

            if (_options.EnableOutbox)
            {
                IContextWithEventOutbox context = _sp.GetRequiredService<IContextWithEventOutbox>();
                if ((await context.Context.Database.GetAppliedMigrationsAsync()).All(x => x != OutboxMigrationName))
                {
                    await MigrateOutboxAsync(context);
                }
            }
        }

        private Task MigrateOutboxAsync(IContextWithEventOutbox context)
        {
            //TODO:throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task MigrateInboxAsync(IContextWithEventInbox context)
        {
            //TODO:throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}