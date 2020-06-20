namespace LabApp.Shared.Dto.Student
{
    public class GroupSubjectDto
    {
        public int Id { get; set; }
        
        public string GroupName { get; set; }

        public SubjectDto Subject { get; set; }
        
        public string Description { get; set; }

        public UserListDto Teacher { get; set; }
    }
}