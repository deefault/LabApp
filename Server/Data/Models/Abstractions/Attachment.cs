using System;
using System.ComponentModel.DataAnnotations;

namespace LabApp.Server.Data.Models.Abstractions
{
    public abstract class Attachment : IJobDeleted, IInsertedTrackable
    {
        [Key] 
        public int Id { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }
        
        public int UserId { get; set; }

        /// <summary>
        /// size in bytes
        /// </summary>
        public long Size { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool ToDelete { get; set; } = false;
        
        public DateTime? DeleteDate { get; set; }

        public DateTime Inserted { get; set; } = DateTime.UtcNow;
        
        public virtual User User { get; set; }
    }
}