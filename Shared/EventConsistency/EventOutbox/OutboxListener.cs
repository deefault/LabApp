using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public interface IOutboxListener
    {
        void OnNewMessage(string id);
        void OnNewMessages(IEnumerable<string> id);
        IAsyncEnumerable<string> GetMessagesAsync(CancellationToken ct);
    }

    public class OutboxListener : ChannelListenerBase, IOutboxListener
    {
        public OutboxListener(ILogger<OutboxListener> logger) : base(logger)
        {
        }
    }
}