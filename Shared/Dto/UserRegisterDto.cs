using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LabApp.Shared.Dto
{
    public abstract class UserRegisterDto 
    {
        [EmailAddress] 
        [Required]
        public string Email { get; set; }
        
        
        [Required]
        public short PhoneCode { get; set; }
        
        [Required]
        public long Phone { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        
        [EmailAddress]
        public string ContactEmail { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Surname { get; set; }
        
        public string Middlename { get; set; }
        
        public DateTime DateBirth { get; set; } = new DateTime(1970,1,1);
        
    }

    public class StudentRegisterDto : UserRegisterDto
    {
        
    }

    public class TeacherRegisterDto : UserRegisterDto
    {
        public int? AcademicRankId { get; set; }
    }
}