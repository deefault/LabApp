using System.ComponentModel.DataAnnotations;

namespace LabApp.Server.Data.Models
{
    public class Subject
    {
        public int Id { get; set; }
        
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int TeacherId { get; set; }
        
        
        public virtual Teacher Teacher { get; set; }
    }
}