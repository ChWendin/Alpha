using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class LoginFormModel
    {

        [Required]
        [Display(Name = "Email", Prompt = "Enter your email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Password", Prompt = "Enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }

}

