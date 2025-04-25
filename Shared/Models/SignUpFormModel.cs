using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class SignUpFormModel
    {
        [Required]
        [Display(Name = "First Name", Prompt = "Enter your full name")]
        public string FullName { get; set; } = null!;

        [Required]
        [RegularExpression(@"^[\w\.\-]+@([\w\-]+\.)+[A-Za-z]{2,}$", ErrorMessage = "Invalid email address")]
        [Display(Name = "Email", Prompt = "Enter your email address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Invalid password")]
        [Display(Name = "Password", Prompt = "Enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm Password", Prompt = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [Range(typeof(bool), "true", "true")]
        public bool Terms { get; set; } 
    }
}
