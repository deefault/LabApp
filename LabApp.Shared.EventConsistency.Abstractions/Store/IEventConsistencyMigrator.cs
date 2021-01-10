using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IEventConsistencyMigrator
    {
        Task MigrateAsync();
    }
}