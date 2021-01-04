using System.Threading.Tasks;

namespace LabApp.Shared.EventConsistency
{
    public interface IFallbackEventProcessor
    {
        Task ProcessAsync();
    }
}