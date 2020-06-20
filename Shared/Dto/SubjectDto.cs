using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LabApp.Shared.Dto
{
    public class SubjectDto
    {
        public int Id { get; set; }
        
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int TeacherId { get; set; }
    }
}