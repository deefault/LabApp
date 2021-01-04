using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency
{
    public interface IEventProcessor
    {
        Task ProcessEventAsync(string id);
    }
}