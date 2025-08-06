namespace CarSpace.Services.Core.Contracts.User.Requests;

public sealed record LoginApplicationUserRequest(
    string Email,
    string Password
);
