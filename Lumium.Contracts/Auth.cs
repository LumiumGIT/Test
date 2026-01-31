namespace Lumium.Contracts;

public record UserInfo(Guid Id, string Email, string FirstName, string LastName);

public record LoginRequest(string TenantIdentifier, string Email, string Password);

public record LoginResponse(string Token, string TenantName, UserInfo User);