using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace BlazorApp.Server.Data.ViewModels
{
    public class UserRegister
    {
        [EmailAddress] [Required] public string Email { get; set; }

        [Required] public short PhoneCode { get; set; }

        [Required] public long Phone { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Surname { get; set; }

        public string Middlename { get; set; }

        [Required] public DateTime DateBirth { get; set; } = new DateTime(1970, 1, 1);

        [Required] [Display(Name = "Country")] public string CountryCode { get; set; }

        public SelectList Countries { get; set; }
    }
}