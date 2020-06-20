using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabApp.Shared.Dto.Teacher
{
    public class AssignmentDto
    {
        public int Id { get; set; }
        
        [Required]
        public int SubjectId { get; set; }
        
        public string SubjectName { get; set; }
        
        [StringLength(100)]
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }

        public decimal MaxScore { get; set; } = 5;

        /// <summary>
        /// Разрешить сдвать после Deadline
        /// </summary>
        public bool AllowAfterDeadline { get; set; } = false;

        /// <summary>
        /// Штраф, если AllowAfterDeadline = true, и работа сдана после Deadline 
        /// </summary>
        public decimal FineAfterDeadline { get; set; } = 0;
        
        public bool IsHidden { get; set; }
        
        public DateTime? Start { get; set; }
		
        public DateTime? DeadLine { get; set; }
        
    }
    
    public class AssignmentDetailsDto : AssignmentDto
    {
        public virtual IEnumerable<AttachmentDto> Attachments { get; set; }
    }
}