using NotificationService.Models.Enums;

namespace NotificationService.DAL.Models
{
    public class UserNotificationOptions
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public NotifierType Type { get; set; }

        public NotificationOptions Options { get; set; }
    }
}