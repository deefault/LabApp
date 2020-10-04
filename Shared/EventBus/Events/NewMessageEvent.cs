using System.Collections.Generic;
using LabApp.Shared.EventBus.Events.Abstractions;

namespace LabApp.Shared.EventBus.Events
{
    public class NewMessageEvent : IntegrationEvent
    {
        public int MessageId { get; set; }
        
        public int UserId { get; set; }
        
        public int ConversationId { get; set; }
        
        public IEnumerable<int> Users { get; set; }

        public string Text { get; set; }
    }
}