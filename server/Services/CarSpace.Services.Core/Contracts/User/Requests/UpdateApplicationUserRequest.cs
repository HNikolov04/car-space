using Microsoft.AspNetCore.Http;

namespace CarSpace.Services.Core.Contracts.User.Requests;

public sealed record UpdateApplicationUserRequest(
    string? NewUsername,
    string? CurrentPassword,
    string? NewPassword,
    string? ConfirmPassword,
    IFormFile? ProfileImage
);
