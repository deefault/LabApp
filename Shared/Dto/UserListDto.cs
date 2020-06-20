using System;
using LabApp.Shared.Enums;

namespace LabApp.Shared.Dto
{
    public class UserListDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Middlename { get; set; }

        public UserType UserType { get; set; }

        public int? PhotoId { get; set; }
        
        public string Thumbnail { get; set; }
    }
}