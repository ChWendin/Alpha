
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Data.Services;

public class UserService(UserManager<MemberEntity> userManager, SignInManager<MemberEntity> signInManager)
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;

    public async Task<IdentityResult> CreateAsync(SignUpFormModel form)
    {
        if (form is null)
            return IdentityResult.Failed(new IdentityError { Description = "Form data saknas." });

        if (await _userManager.Users.AnyAsync(u => u.Email == form.Email))
            return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "E‐postadressen finns redan." });

        var user = new MemberEntity
        {
            UserName = form.Email,
            Email = form.Email,
            FullName = form.FullName
        };

        return await _userManager.CreateAsync(user, form.Password);
    }

}
