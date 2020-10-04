using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Claims;

namespace NotificationService.Hubs
{
    public class HubUserProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}