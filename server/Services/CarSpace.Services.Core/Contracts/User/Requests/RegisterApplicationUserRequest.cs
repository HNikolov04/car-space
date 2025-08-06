namespace CarSpace.Services.Core.Contracts.User.Requests;

public sealed record RegisterApplicationUserRequest(
    string Email,
    string Username,
    string Password
);
