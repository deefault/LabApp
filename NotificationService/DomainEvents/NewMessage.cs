using System;
using System.Collections.Generic;
using NotificationService.Abstractions;

namespace NotificationService.DomainEvents
{
    public class NewMessage : INotificationEvent
    {
        public int MessageId { get; set; }
        
        public int UserId { get; set; }
        
        public int ConversationId { get; set; }

        public DateTime Sent { get; set; }
        
        public IEnumerable<int> Users { get; set; } = new List<int>();

        public string Text { get; set; }
    }
}