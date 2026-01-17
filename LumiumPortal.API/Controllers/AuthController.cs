using Lumium.Application.Common.Interfaces;
using Lumium.Infrastructure.MultiTenancy;
using Lumium.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumiumPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    MasterDbContext masterContext,
    ApplicationDbContext appContext,
    IJwtService jwtService,
    ITenantContext tenantContext)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Nađi tenant po IDENTIFIER-u (user-friendly)
        var tenant = await masterContext.Tenants
            .FirstOrDefaultAsync(t => 
                t.Identifier == request.TenantIdentifier && t.IsActive);

        if (tenant == null)
        {
            return Unauthorized(new { message = "Invalid tenant" });
        }

        // 2. Postavi tenant context (koristi ID)
        ((TenantContext)tenantContext).SetTenant(
            tenant.Id.ToString(),  // ← ID u context
            tenant.SchemaName);

        // 3. Nađi korisnika
        var user = await appContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // 4. JWT sadrži tenant ID (ne identifier)
        var token = jwtService.GenerateToken(
            user.Id, 
            user.Email, 
            tenant.Id.ToString());

        return Ok(new { token, user });
    }
}

public record LoginRequest(string Email, string TenantIdentifier);