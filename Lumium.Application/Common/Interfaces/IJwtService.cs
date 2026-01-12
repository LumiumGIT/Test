namespace Lumium.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, string tenantId);
    bool ValidateToken(string token);
}