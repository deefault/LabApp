using System;
using System.Collections.Generic;
using LabApp.Shared.Enums;

namespace LabApp.Shared.Dto.Student
{
    public class AssignmentDtoStudent
    {
        public int Id { get; set; }
        
        public int SubjectId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public DateTime Start { get; set; }
        
        public DateTime Deadline { get; set; }

        public decimal MaxScore { get; set; } = 5;
        
        public bool AllowAfterDeadline { get; set; } = false;
        
        public decimal FineAfterDeadline { get; set; } = 0;

        public int? StudentAssignmentId { get; set; }
        
        public AssignmentStatus? StudentAssignmentStatus { get; set; }
    }
    
    public class AssignmentDetailsDtoStudent : AssignmentDtoStudent
    {

        
        /// <summary>
        /// Concrete assignment by student (may be null if no such)
        /// </summary>
        public StudentAssignmentDto StudentAssignment { get; set; }

        public virtual IEnumerable<AttachmentDto> Attachments { get; set; }
    }
}