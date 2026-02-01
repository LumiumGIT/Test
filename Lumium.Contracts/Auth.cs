namespace Lumium.Contracts;

#region Portal

public record UserInfo(Guid Id, string Email, string FirstName, string LastName);

public record LoginRequest(string TenantIdentifier, string Email, string Password);

public record LoginResponse(string Token, string TenantName, UserInfo User);

#endregion

#region Admin

public record AdminUserInfo(Guid Id, string Email, string FirstName, string LastName, string Role);

public record AdminLoginRequest(string Email, string Password);

public record AdminLoginResponse(string Token, AdminUserInfo User);

#endregion