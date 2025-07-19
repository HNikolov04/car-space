using Microsoft.AspNetCore.Identity;

namespace CarSpace.Services.Core.Interfaces;

public interface IUserService
{
    Task<IdentityResult> RegisterAsync(string email, string username, string password);
    Task<SignInResult> LoginAsync(string email, string password);
}
