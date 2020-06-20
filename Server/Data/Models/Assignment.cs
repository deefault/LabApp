using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using LabApp.Server.Data.Models.Attachments;
using LabApp.Server.Data.Models.ManyToMany;

namespace LabApp.Server.Data.Models
{
    /// <summary>
    /// Работа (Дз, лабораторная)
    /// </summary>
    public class Assignment
    {
        public int Id { get; set; }
        
        [Required]
        public int SubjectId { get; set; }
        
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

        public int TeacherId { get; set; }

        
        public virtual Teacher Teacher { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual IEnumerable<StudentAssignment> Assignments { get; set; }

        public virtual IEnumerable<GroupAssignment> Groups { get; set; }

        public virtual IEnumerable<AssignmentAttachment> Attachments { get; set; }
    }
}