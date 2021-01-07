using LabApp.Shared.EventConsistency.Abstractions;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public class OutboxListener : ChannelListenerBase, IOutboxListener
    {
        public OutboxListener(ILogger<OutboxListener> logger) : base(logger)
        {
        }
    }
}