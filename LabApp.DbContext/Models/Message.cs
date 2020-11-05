using System;
using System.Collections.Generic;
using LabApp.Server.Data.Models.Attachments;

namespace LabApp.Server.Data.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime Sent { get; set; } = DateTime.UtcNow;
        
        public int ConversationId { get; set; }
        
        
        public virtual Conversation Conversation { get; set; }
        
        public virtual User User { get; set; }

        public virtual IEnumerable<MessageAttachment> Attachments { get; set; }
    }
}