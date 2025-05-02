using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Auth;
using Shared.Models;
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService _userService;
        private readonly SignInManager<MemberEntity> _signInManager;
        public LoginController(UserService userService, SignInManager<MemberEntity> signInManager)
        {
            _userService = userService;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var vm = new LoginViewModel
            {
                FormData = new LoginFormModel()
            };
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(vm.FormData.Email, vm.FormData.Password, false, false);
            if(result.Succeeded)
            return RedirectToAction("Projects", "Projects");

            ModelState.AddModelError(string.Empty, "Incorrect email or password.");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Login");
        }
    }
}