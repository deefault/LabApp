using System.ComponentModel.DataAnnotations;
using LabApp.Shared.Enums;

namespace LabApp.Server.Data.Models
{
    public partial class UserIdentity
    {
        [Key]
        public int UserId { get; set; }

        public short PhoneCode { get; set; }

        public long Phone { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public bool IsVerified { get; set; }

        [Required]
        [StringLength(44)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(24)]
        public string PasswordSalt { get; set; }
        
        // dublicates User.Type discriminator
        public UserType Role { get; set; }

        public virtual User User { get; set; }
    }
}