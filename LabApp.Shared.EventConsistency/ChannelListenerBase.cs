using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace LabApp.Shared.EventConsistency
{
    public class ChannelListenerBase
    {
        private readonly Channel<string> _channel;
        private readonly ILogger<ChannelListenerBase> _logger;

        public ChannelListenerBase(ILogger<ChannelListenerBase> logger)
        {
            _logger = logger;
            _channel = Channel.CreateUnbounded<string>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
        }

        public void OnNewMessage(string id)
        {
            if (!_channel.Writer.TryWrite(id))
            {
                _logger.LogDebug("Cannot write to channel, id: {Id}", id);
            }
        }
        
        public void OnNewMessages(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                if (!_channel.Writer.TryWrite(id))
                {
                    _logger.LogDebug("Cannot write to channel, id: {Id}", ids);
                }   
            }
        }

        public IAsyncEnumerable<string> GetMessagesAsync(CancellationToken ct) 
            => _channel.Reader.ReadAllAsync(ct);
    }
}