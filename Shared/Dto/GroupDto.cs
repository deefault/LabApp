using System.ComponentModel.DataAnnotations;

namespace LabApp.Shared.Dto
{
    public class GroupDto
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string GroupName { get; set; }
        
        public int SubjectId { get; set; }
        
        public byte? StudyYear { get; set; }
        
        public int TeacherId { get; set; }
    }
    
    public class GroupDetailsTeacherDto
    {
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string GroupName { get; set; }
        
        public string InviteCode { get; set; }
        
        public int SubjectId { get; set; }
        
        public byte? StudyYear { get; set; }
        
        public int TeacherId { get; set; }
    }
}