using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabApp.Server.Data.Models.Abstractions;

namespace LabApp.Server.Data.Models
{
    public class PostPhoto : Image
    {
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty("PostPhotos")]
        public virtual Post Post { get; set; }
    }
}