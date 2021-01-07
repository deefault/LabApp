using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventInbox
{
    public interface IInboxListener
    {
        void OnNewMessage(string id);
        IAsyncEnumerable<string> GetMessagesAsync(CancellationToken ct);
    }

    public class InboxListener : ChannelListenerBase, IInboxListener
    {
        public InboxListener(ILogger<InboxListener> logger) : base(logger)
        {
        }
    }
}