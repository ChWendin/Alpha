using Microsoft.AspNetCore.Mvc;
using WebApp.Models.Auth;
using Shared.Models;
using Data.Services;

namespace WebApp.Controllers
{
    public class SignUpController : Controller
    {
        private readonly UserService _userService;

        public SignUpController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
           
            var vm = new SignUpViewModel();
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _userService.CreateAsync(vm.FormData);
            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(string.Empty, err.Description);
                return View(vm);
            }

            return RedirectToAction("Login", "Login");
        }
    }
}
