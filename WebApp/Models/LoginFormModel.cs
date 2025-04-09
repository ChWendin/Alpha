using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class LoginFormModel
    {

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }

}

