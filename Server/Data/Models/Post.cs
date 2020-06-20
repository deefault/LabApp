using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models
{
    /// <summary>
    /// Teacher's post
    /// </summary>
    public class Post : IInsertedTrackable
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Posts")]
        public virtual Teacher User { get; set; }

        [InverseProperty(nameof(Post))]
        public virtual IEnumerable<PostPhoto> PostPhotos { get; set; }

        [InverseProperty(nameof(Post))]
        public virtual IEnumerable<PostLike> Likes { get; set; }

        public DateTime Inserted { get; set; } = DateTime.UtcNow;
    }
}