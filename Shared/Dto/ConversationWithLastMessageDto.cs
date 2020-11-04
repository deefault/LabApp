using LabApp.Shared.Enums;

namespace LabApp.Shared.Dto
{
    public class ConversationWithLastMessageDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ConversationType Type { get; set; }

        public MessageDto LastMessage { get; set; }
        
        public UserListDto LastMessageUser { get; set; }
        
        public int NewMessages { get; set; }
    }
}