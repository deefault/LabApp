using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabApp.Server.Data.Models.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LabApp.Shared.Enums;


namespace LabApp.Server.Data.Models
{
    public abstract class User : ISoftDeletable
    {
        [Key]
        [BindNever]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        
        [StringLength(50)]
        public string Middlename { get; set; }

        /// <summary>
        /// public email
        /// </summary>
        [EmailAddress]
        [StringLength(100)]
        public string ContactEmail { get; set; }

        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public UserType UserType { get; }

        public DateTime DateBirth { get; set; }

        public int? PhotoId { get; set; }

        /// <summary>
        /// Main photo
        /// </summary>
        [ForeignKey("PhotoId")]
        public UserPhoto MainPhoto { get; set; }
        
        public string Thumbnail { get; set; }

        [ForeignKey("UserId")]
        [BindNever]
        public UserIdentity UserIdentity { get; set; }

        public IEnumerable<UserPhoto> Photos { get; set; }

        public string FullName => $"{Name} {Surname}";

        public bool IsDeleted { get; set; }
    }
}