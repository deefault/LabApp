using System;
using System.ComponentModel.DataAnnotations;

namespace LabApp.Server.Data.Models.Abstractions
{
    public abstract class Image : IJobDeleted, IInsertedTrackable
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public string Name { get; set; }

        /// <summary>
        ///     size in bytes
        /// </summary>
        public int Size { get; set; }

        public bool IsDeleted { get; set; }

        public bool ToDelete { get; set; }

        public DateTime? DeleteDate { get; set; }

        public DateTime Inserted { get; set; } = DateTime.UtcNow;
    }
}