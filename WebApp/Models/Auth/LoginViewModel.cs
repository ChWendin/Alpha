using Microsoft.AspNetCore.Mvc;
using Shared.Models;

namespace WebApp.Models.Auth
{
    public class LoginViewModel
    {
        public LoginFormModel FormData { get; set; } = new();
    }
}
