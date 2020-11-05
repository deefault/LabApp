using System.Collections.Generic;
using LabApp.Server.Data.Models.Dictionaries;

namespace LabApp.Server.Data.Models
{
    public class Teacher : User
    {
        public int? AcademicRankId { get; set; }

        public virtual AcademicRank AcademicRank { get; set; }

        public virtual IEnumerable<Group> Groups { get; set; }
        
        public virtual IEnumerable<Post> Posts { get; set; }
    }
}