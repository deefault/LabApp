using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabApp.Shared.Dto
{
    public abstract class ProfileDto
    {
        public int UserId { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string Middlename { get; set; }
        
        public string ContactEmail { get; set; }

        public DateTime DateBirth { get; set; }

        public ImageDto MainPhoto { get; set; }

        public string FullName => $"{Name} {Surname}";
    }

    public class TeacherProfileDto : ProfileDto
    {
        public string AcademicRankName { get; set; }
        public int? AcademicRankId { get; set; }
    }
    
    public class StudentProfileDto : ProfileDto
    {
        
    }
}