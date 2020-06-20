using System;
using System.ComponentModel.DataAnnotations;

namespace LabApp.Server.Data.Models.ManyToMany
{
    /// <summary>
    /// User participant in conversation
    /// </summary>
    public class UserConversation
    {
        public int UserId { get; set; }
        
        public int ConversationId { get; set; }
        
        public DateTime? LastReadMessage { get; set; }


        public virtual User User { get; set; }
        
        public virtual Conversation Conversation { get; set; }
    }
}