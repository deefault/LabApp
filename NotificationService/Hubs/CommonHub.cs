using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Models.Dto;

namespace NotificationService.Hubs
{
    public interface ICommonHubClient
    {
        Task NewMessage(NewMessageDto obj);
    }

    
    [Authorize]
    public class CommonHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public Task NewMessage(NewMessageDto @event)
        {
            return Clients.Users(@event.Users.Select(x => x.ToString()))
                .SendAsync(nameof(ICommonHubClient.NewMessage), new NewMessageDto());
        }
        
    }
}