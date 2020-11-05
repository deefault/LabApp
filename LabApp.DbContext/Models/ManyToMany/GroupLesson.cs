using System;

namespace LabApp.Server.Data.Models.ManyToMany
{
    public class GroupLesson
    {
        public DateTime? DateTime { get; set; } = System.DateTime.UtcNow;

        public bool IsHidden { get; set; } = false;
        
        public int GroupId { get; set; }
        
        public int LessonId { get; set; }
        
        
        public virtual Group Group { get; set; }
        
        public virtual Lesson Lesson { get; set; }
    }
}