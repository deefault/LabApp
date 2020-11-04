using LabApp.Server.Data.Models;
using LabApp.Shared.Enums;

namespace LabApp.Server.Data.QueryModels
{
    public class ConversationWithLastMessage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ConversationType Type { get; set; }

        public Message LastMessage { get; set; }
        
        public User LastMessageUser { get; set; }
        
        public int NewMessages { get; set; }
    }
}