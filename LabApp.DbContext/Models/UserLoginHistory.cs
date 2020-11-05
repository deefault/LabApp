using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using LabApp.Server.Data.Models.Interfaces;

namespace LabApp.Server.Data.Models
{
    public class UserLoginHistory : IInsertedTrackable
    {
        [Key]
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        //public string Token { get; set; }
        [Required]
        public IPAddress Address { get; set; }
        
        [MaxLength(1000)]
        public string UserAgent { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public DateTime Inserted { get; set; } = DateTime.UtcNow;
    }
}