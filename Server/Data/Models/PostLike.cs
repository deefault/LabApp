using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabApp.Server.Data.Models
{
    public class PostLike
    {
        public int PostId { get; set; }
        
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}