namespace Lumium.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, string tenantId, string schemaName,  string firstName, string lastName);
    bool ValidateToken(string token);
}