using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models.Attachments
{
    /// <summary>
    /// прикрепления для работы
    /// </summary>
    public class AssignmentAttachment : Attachment
    {
        public int? AssignmentId { get; set; }
        
        /// <summary>
        /// Скрыто для студентов
        /// </summary>
        public bool IsTeacherOnly { get; set; }
        
        public virtual Assignment Assignment { get; set; }
    }
}