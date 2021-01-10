using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF.EventOutbox
{
    public interface IContextWithEventOutbox : IHasDbContext
    {
        bool SaveEventsOnSaveChanges { get; set; }
        DbSet<OutboxEventMessage> EventOutbox { get; set; }
    }
}