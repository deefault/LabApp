using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models.Attachments
{
    public class LessonAttachment : Attachment
    {
        public int? LessonId { get; set; }
        
        public virtual Lesson Lesson { get; set; }
    }
}