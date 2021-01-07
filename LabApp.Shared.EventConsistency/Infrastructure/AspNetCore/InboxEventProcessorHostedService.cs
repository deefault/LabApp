using System.Threading;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.EventInbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    public class InboxEventProcessorHostedService : IHostedService
    {
        private readonly IInboxListener _listener;
        private readonly IInboxEventProcessor _eventProcessor;
        private readonly IServiceScope _scope;


        public InboxEventProcessorHostedService(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            _listener = _scope.ServiceProvider.GetRequiredService<IInboxListener>();
            _eventProcessor = _scope.ServiceProvider.GetRequiredService<IInboxEventProcessor>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await foreach (var id in _listener.GetMessagesAsync(cancellationToken))
            {
                await _eventProcessor.ProcessAsync(id);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }

        private void Dispose()
        {
            _scope?.Dispose();
        }
    }
}