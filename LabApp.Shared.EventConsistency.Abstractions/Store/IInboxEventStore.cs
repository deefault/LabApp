using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IInboxEventStore : IEventStore
    {
        Task IncrementFailedAsync(InboxEventMessage eventMessage);
    }
}