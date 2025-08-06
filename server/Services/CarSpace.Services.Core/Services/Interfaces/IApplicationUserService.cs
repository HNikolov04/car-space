using CarSpace.Services.Core.Contracts.User.Requests;
using CarSpace.Services.Core.Contracts.User.Responses;
using Microsoft.AspNetCore.Identity;

namespace CarSpace.Services.Core.Services.Interfaces;

public interface IApplicationUserService
{
    Task<IdentityResult> RegisterAsync(RegisterApplicationUserRequest request);
    Task<LoginApplicationUserResponse?> LoginAsync(LoginApplicationUserRequest request);
    Task<MeUserResponse?> GetUserByIdAsync(Guid userId);
    Task<bool> UpdateUserAsync(Guid userId, UpdateApplicationUserRequest request);
    Task<bool> SoftDeleteUserAsync(Guid userId);
}
