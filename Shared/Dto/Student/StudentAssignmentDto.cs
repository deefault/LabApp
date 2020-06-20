using System;
using System.Collections.Generic;
using LabApp.Shared.Enums;

namespace LabApp.Shared.Dto.Student
{
    public class StudentAssignmentDto
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        public int StudentId { get; set; }

        public AssignmentStatus Status { get; set; } = AssignmentStatus.Submitted;

        public string Body { get; set; }

        public decimal? Score { get; set; }

        public decimal? FineScore { get; set; }
        
        public int GroupId { get; set; }
        
        public DateTime? Submitted { get; set; }
        
        public DateTime? LastStatusChange { get; set; }
        
        public DateTime? Completed { get; set; }


        public virtual IEnumerable<AttachmentDto> Attachments { get; set; }
    }
}