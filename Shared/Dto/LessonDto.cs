using System.Collections.Generic;

namespace LabApp.Shared.Dto
{
    public class LessonDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public int Order { get; set; } = 0;

        /// <summary>
        /// Практическое занятие
        /// </summary>
        public bool IsPractical { get; set; }

        public int SubjectId { get; set; }
        
        public string SubjectName { get; set; }

        public int? AssignmentId { get; set; } = null;
        
        public List<AttachmentDto> Attachments { get; set; }
        
    }
}