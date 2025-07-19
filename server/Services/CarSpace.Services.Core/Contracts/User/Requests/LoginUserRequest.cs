namespace CarSpace.Services.Core.Contracts.User.Requests;

public sealed record LoginUserRequest(
    string Email,
    string Password
);
