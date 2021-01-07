using System.Threading;
using System.Threading.Tasks;
using LabApp.Shared.EventConsistency.Abstractions;
using LabApp.Shared.EventConsistency.EventOutbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LabApp.Shared.EventConsistency.Infrastructure.AspNetCore
{
    public class OutboxEventProcessorHostedService : IHostedService
    {
        private readonly IOutboxListener _listener;
        private readonly IOutboxEventProcessor _eventProcessor;
        private readonly IServiceScope _scope;

        public OutboxEventProcessorHostedService(IServiceScopeFactory scopeFactory)
        {
            _scope = scopeFactory.CreateScope();
            _listener = _scope.ServiceProvider.GetRequiredService<IOutboxListener>();
            _eventProcessor = _scope.ServiceProvider.GetRequiredService<IOutboxEventProcessor>();
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

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}