namespace CarSpace.Services.Core.Contracts.User.Responses;

public record MeUserResponse(
    Guid Id,
    string Email,
    string Username,
    string? ProfileImageUrl,
    string Role
);
