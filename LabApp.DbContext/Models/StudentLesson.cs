namespace LabApp.Server.Data.Models
{
    public class StudentLesson
    {
        public int Id { get; set; }
        
        public int StudentId { get; set; }
        
        public int LessonId { get; set; }

        public int Score { get; set; } = 0;
        
        
        public virtual Student Student { get; set; }
        
        public virtual Lesson Lesson { get; set; }
    }
}