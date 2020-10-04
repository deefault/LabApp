using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using NotificationService.Models.Dto;

namespace NotificationService.Services
{
    public interface IRealtimeNotificationService
    {
        Task NewMessage(NewMessageDto obj);
    }
    
    public class SignalRService : IRealtimeNotificationService
    {
        private readonly IHubContext<CommonHub> _hub;
        
        public SignalRService(IHubContext<CommonHub> hub)
        {
            _hub = hub;
        }

        public Task NewMessage(NewMessageDto obj)
        {
            return _hub.Clients.Users(obj.Users.Select(x => x.ToString())).SendAsync("NewMessage", obj);
        }
    }
}