using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency.EventOutbox
{
    public class OutboxListener : ChannelListenerBase
    {
        public OutboxListener(ILogger<OutboxListener> logger) : base(logger)
        {
        }
    }
}