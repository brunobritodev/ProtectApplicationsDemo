using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProtectApplication.Jwt.Models
{
    public class AuthJwtResponse
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
    }

    public class UserRegister
    {
        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must have between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords doesn't match.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "The {0} is required")]
        [EmailAddress(ErrorMessage = "The {0} is in a incorrect format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} is required")]
        [StringLength(100, ErrorMessage = "The {0} must have between {2} and {1} characters", MinimumLength = 6)]
        public string Password { get; set; }
    }
}
