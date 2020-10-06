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
    public class CommonHub : Hub<ICommonHubClient>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}