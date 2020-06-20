using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace LabApp.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {
        }

        public async Task Notify(string userId, string message)
        {
            await Clients.Client(userId).SendAsync("Notify", message);
        }
    }
}