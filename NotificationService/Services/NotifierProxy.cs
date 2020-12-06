using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NotificationService.Abstractions;
using NotificationService.DomainEvents;
using NotificationService.Models.Enums;

namespace NotificationService.Services
{
    public class NotifierProxy : INotifier
    {
        private readonly IEnumerable<INotifier> _notifiers;
        private readonly ILogger<NotifierProxy> _logger;

        public NotifierType Type => NotifierType.None;

        public NotifierProxy(IEnumerable<INotifier> notifiers, ILogger<NotifierProxy> logger)
        {
            _notifiers = notifiers;
            _logger = logger;
        }

        public Task NewMessage(NewMessage @event)
        {
            return RunForEachNotifier(notifier => notifier.NewMessage(@event), @event);
        }

        private async Task RunForEachNotifier(Func<INotifier, Task> action, INotificationEvent @event)
        {
            foreach (var notifier in _notifiers)
            {
                try
                {
                    _logger.LogTrace("Started notifying for {@NotificationEvent} for notifier {NotifierType}", @event,
                        notifier.Type);

                    await action(notifier);

                    _logger.LogTrace("Finished notifying for {@NotificationEvent} for notifier {NotifierType}", @event,
                        notifier.Type);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error for event: {@NotificationEvent} for notifier: {NotifierType}", @event,
                        notifier.Type);
                }
            }
        }
    }
}