using Microsoft.EntityFrameworkCore;

namespace LabApp.Shared.EventConsistency.Stores.EF
{
    public interface IHasDbContext
    {
        DbContext Context { get; }
    }
}