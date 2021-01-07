using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency
{
    public interface IEventProcessor
    {
        Task ProcessAsync(string id);
    }
}