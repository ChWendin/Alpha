using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Auth
{
    public class SignUpFormModel
    {
        public string FullName { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
