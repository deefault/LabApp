using System;
using System.Collections.Generic;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Shared.Enums;

namespace LabApp.Server.Data.Models
{
    public class StudentAssignment
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }
        
        public int GroupId { get; set; }

        public int StudentId { get; set; }

        public AssignmentStatus Status { get; set; } = AssignmentStatus.Submitted;
        
        public string Body { get; set; }
        
        /// <summary>
        /// Комментарий от преподавателя для себя
        /// </summary>
        public string TeacherComment { get; set; }

        public decimal? Score { get; set; }

        /// <summary>
        /// штраф
        /// </summary>
        public decimal? FineScore { get; set; }

        public DateTime? Submitted { get; set; }
        
        public DateTime? LastStatusChange { get; set; }
        
        public DateTime? Completed { get; set; }
        

        public virtual Assignment Assignment { get; set; }

        public virtual Student Student { get; set; }
        
        public virtual Group Group { get; set; }

        /// <summary>
        /// Вложения студента
        /// </summary>
        public virtual IEnumerable<StudentAssignmentAttachment> Attachments { get; set; }
    }
}