using System.Collections.Generic;
using System.Threading;

namespace LabApp.Shared.EventConsistency.Abstractions
{
    public interface IOutboxListener
    {
        void OnNewMessage(string id);
        void OnNewMessages(IEnumerable<string> id);
        IAsyncEnumerable<string> GetMessagesAsync(CancellationToken ct);
    }
}