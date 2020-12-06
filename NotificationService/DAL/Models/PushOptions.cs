namespace NotificationService.DAL.Models
{
    public class PushOptions : NotificationOptions
    {
        public string[] Tokens { get; set; }
    }
}