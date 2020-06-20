using System.Collections.Generic;
using LabApp.Server.Data.Models.ManyToMany;

namespace LabApp.Server.Data.Models
{
    public class Student : User
    {
        public virtual IEnumerable<StudentGroup> Groups { get; set; }
    }
}