using System;
using System.ComponentModel.DataAnnotations;
using LabApp.Server.Data.Models.ManyToMany;

namespace LabApp.Server.Data.Models
{
    public class AdditionalScore
    {
        public int Id { get; set; }
        
        public int StudentId { get; set; }
        
        public int GroupId { get; set; }
        
        [StringLength(1000)]
        public string Comment { get; set; }
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
        
        public virtual Student Student { get; set; }
        
        public virtual Group Group { get; set; }
    }
}