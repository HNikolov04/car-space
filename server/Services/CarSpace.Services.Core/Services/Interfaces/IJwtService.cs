namespace CarSpace.Services.Core.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, string role);
}
