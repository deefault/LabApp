using System.Collections.Generic;

namespace NotificationService.Models.Dto
{
    public class NewMessageDto
    {
        public int MessageId { get; set; }
        
        public int UserId { get; set; }
        
        public int ConversationId { get; set; }
        
        public IEnumerable<int> Users { get; set; } = new List<int>();

        public string Text { get; set; }
    }
}