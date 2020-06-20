using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabApp.Server.Data.Models.ManyToMany;

namespace LabApp.Server.Data.Models
{
    public class Group
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string GroupName { get; set; }
        
        public int SubjectId { get; set; }

        public int TeacherId { get; set; }
        
        [StringLength(10)]
        public string InviteCode { get; set; }

        public byte? StudyYear { get; set; }


        public virtual Subject Subject { get; set; }
        
        public virtual Teacher Teacher { get; set; } 
        
        public virtual IEnumerable<StudentGroup> Students { get; set; }
    }
}