using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace LabApp.Server.Hubs
{
    public interface ICommonHubClient
    {
        Task NewMessage(int userId, int messageId, string messageText, int conversationConversationId, string userFullName);
    }

    [Authorize]
    public class CommonHub : Hub<ICommonHubClient>
    {
        public Task NewMessage(int userId, int messageId, string messageText, int conversationConversationId, string userFullName)
        {
            return Clients.Client(userId.ToString())
                .NewMessage(userId, messageId, messageText, conversationConversationId, userFullName);
        }
    }
}