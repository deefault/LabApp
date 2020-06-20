using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models.Attachments
{
    public class StudentAssignmentAttachment : Attachment
    {
        public int? AssignmentId { get; set; }

        [ForeignKey("AssignmentId")]
        public virtual StudentAssignment Assignment { get; set; }
    }
}