using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LabApp.Server.Data.Models.Attachments;

namespace LabApp.Server.Data.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; } 
        
        public int Order { get; set; }

        /// <summary>
        /// Практическое занятие
        /// </summary>
        public bool IsPractical { get; set; }

        public int SubjectId { get; set; }

        public int? AssignmentId { get; set; }

        public virtual Assignment Assignment { get; set; }
        
        public virtual IEnumerable<LessonAttachment> Attachments { get; set; }

        public virtual Subject Subject { get; set; }
    }
}