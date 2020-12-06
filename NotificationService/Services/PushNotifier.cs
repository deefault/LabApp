using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NotificationService.Abstractions;
using NotificationService.DAL.Models;
using NotificationService.DomainEvents;
using NotificationService.Models.Enums;
using NotificationService.Repositories;

namespace NotificationService.Services
{
    public class PushNotifier : INotifier
    {
        private readonly ILogger<PushNotifier> _logger;
        private readonly IUserOptionsRepository _userOptions;

        public PushNotifier(ILogger<PushNotifier> logger, IUserOptionsRepository userOptions)
        {
            _logger = logger;
            _userOptions = userOptions;
        }

        public NotifierType Type => NotifierType.Push;

        public async Task NewMessage(NewMessage @event)
        {
            var options = await _userOptions.GetUserOptionsAsync(@event.Users, NotifierType.Push);
            var tokens = options.Select(x => x.Options).Cast<PushOptions>()
                .SelectMany(x => x.Tokens.DefaultIfEmpty()).ToList();
            
            // TODO: real push notification using azure notification hub, firebase fcm, etc...
            foreach (var token in tokens)
            {
                _logger.LogInformation("New message Push! ({PushToken})", token);
            }
        }
    }
}