using CarSpace.Data.Models.Entities.User;
using CarSpace.Services.Common.Messages;
using CarSpace.Services.Core.Constants;
using CarSpace.Services.Core.Contracts.User.Requests;
using CarSpace.Services.Core.Contracts.User.Responses;
using CarSpace.Services.Core.Exceptions;
using CarSpace.Services.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarSpace.Services.Core.Services;

public class ApplicationUserService : IApplicationUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IImageService _imageService;

    private const string SUB_FOLDER = ImageServiceConstants.USER_PROFILE_SUB_FOLDER;
    private const string DEFAULT_IMAGE = ImageServiceConstants.USER_PROFILE_IMAGE;

    public ApplicationUserService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService,
        IImageService imageService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _imageService = imageService;
    }

    public async Task<IdentityResult> RegisterAsync(RegisterApplicationUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Username))
        {
            throw new ValidationAppException(ExceptionMessages.InvalidUserData);
        }

        var user = new ApplicationUser(Guid.NewGuid(), request.Email, request.Username, DEFAULT_IMAGE);

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return result;
        }

        var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

        if (!addToRoleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return IdentityResult.Failed(addToRoleResult.Errors.ToArray());
        }

        return result;
    }

    public async Task<LoginApplicationUserResponse> LoginAsync(LoginApplicationUserRequest request)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && !u.IsDeleted);

        if (user is null)
        {
            throw new NotFoundException(ExceptionMessages.UserNotFound);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new ValidationAppException(ExceptionMessages.InvalidLoginCredentials);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        var token = _jwtService.GenerateToken(user.Id, user.Email, role);

        return new LoginApplicationUserResponse(
            user.Id,
            token,
            user.Email,
            user.UserName ?? string.Empty,
            user.ImageUrl ?? string.Empty,
            role
        );
    }

    public async Task<MeUserResponse> GetUserByIdAsync(Guid userId)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user is null)
        {
            throw new NotFoundException(ExceptionMessages.UserNotFound);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "User";

        return new MeUserResponse(
            user.Id,
            user.Email,
            user.UserName,
            user.ImageUrl,
            role
        );
    }

    public async Task<bool> UpdateUserAsync(Guid userId, UpdateApplicationUserRequest request)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user is null)
        {
            throw new NotFoundException(ExceptionMessages.UserNotFound);
        }

        if (!string.IsNullOrWhiteSpace(request.NewUsername))
        {
            user.UserName = request.NewUsername;
        }

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword) ||
                request.NewPassword != request.ConfirmPassword)
            {
                throw new ValidationAppException(ExceptionMessages.InvalidPasswordUpdate);
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, request.CurrentPassword, lockoutOnFailure: false);

            if (!passwordCheck.Succeeded)
            {
                throw new ValidationAppException(ExceptionMessages.InvalidCurrentPassword);
            }

            var passwordResult = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!passwordResult.Succeeded)
            {
                throw new ValidationAppException(ExceptionMessages.PasswordUpdateFailed);
            }
        }

        user.ImageUrl = await _imageService.UpdateImageIfProvidedAsync(
            request.ProfileImage,
            user.ImageUrl,
            user.Id,
            SUB_FOLDER
        );

        var updateResult = await _userManager.UpdateAsync(user);

        return updateResult.Succeeded;
    }

    public async Task<bool> SoftDeleteUserAsync(Guid userId)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

        if (user is null)
        {
            throw new NotFoundException(ExceptionMessages.UserNotFound);
        }

        user.IsDeleted = true;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }
}
