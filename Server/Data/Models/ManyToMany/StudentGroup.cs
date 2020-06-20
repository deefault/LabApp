using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabApp.Server.Data.Models.ManyToMany
{
    public class StudentGroup
    {
        public int GroupId { get; set; }
        
        public int StudentId { get; set; }
        
        
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
     
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}