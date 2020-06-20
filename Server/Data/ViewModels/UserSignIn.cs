using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlazorApp.Server.Data.Models;
using BlazorApp.Server.Services.Identity;

namespace BlazorApp.Server.Data.ViewModels
{
    public class UserSignIn : IValidatableObject
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Login", Description = "Your email")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public UserIdentity User { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (User == null)
            {
                yield return new ValidationResult("User doesnt exists with this Username", new[] {"Username"});
                yield break;
            }

            if (!PasswordHasher.Validate(Password, User.PasswordSalt, User.PasswordSalt))
            {
                yield return new ValidationResult("Password mismatch", new[] {"Password"});
                yield break;
            }
        }
    }
}