using CarSpace.Data.Models.Entities.User;
using CarSpace.Services.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CarSpace.Services.Core;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<IdentityResult> RegisterAsync(string email, string username, string password)
    {
        var user = new ApplicationUser(email, username);

        return await _userManager.CreateAsync(user, password);
    }

    public async Task<SignInResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            return SignInResult.Failed;
        }

        return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
    }
}
