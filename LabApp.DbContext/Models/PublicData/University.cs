using System.ComponentModel.DataAnnotations;

namespace LabApp.Server.Data.Models.PublicData
{
    public class University
    {
        [Key]
        public int Id { get; set; }
        
        [StringLength(150)]
        public int Name { get; set; }
    }
}