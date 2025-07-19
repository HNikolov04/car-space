namespace CarSpace.Services.Core.Contracts.User.Requests;

public sealed record RegisterUserRequest(
    string Email,
    string Username,
    string Password
);
