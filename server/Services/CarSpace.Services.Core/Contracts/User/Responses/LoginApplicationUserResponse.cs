namespace CarSpace.Services.Core.Contracts.User.Responses;

public sealed record LoginApplicationUserResponse(
    Guid Id,
    string JwtToken,
    string Email,
    string Username,
    string ImageUrl,
    string Role
);

