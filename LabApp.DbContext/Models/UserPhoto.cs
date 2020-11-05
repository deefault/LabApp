using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models
{
    public class UserPhoto : Image
    {
        public int UserId { get; set; }
        
        public virtual User User { get; set; }
    }
}