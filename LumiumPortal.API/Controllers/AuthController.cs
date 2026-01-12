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
        // 1. Nađi tenant u master DB
        var tenant = await masterContext.Tenants
            .FirstOrDefaultAsync(t => t.Identifier == request.TenantIdentifier  && t.IsActive);

        if (tenant == null)
        {
            return Unauthorized(new { message = "Invalid tenant" });
        }

        // 2. Postavi tenant context
        ((TenantContext)tenantContext).SetTenant(tenant.Id.ToString(), tenant.SchemaName);

        // 3. Nađi korisnika u tenant schema
        var user = await appContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.IsActive);

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // TODO: Ovde kasnije dodati password verification
        // Za sada preskačemo proveru password-a

        // 4. Generiši JWT token
        var token = jwtService.GenerateToken(user.Id, user.Email, user.TenantId);

        return Ok(new
        {
            token,
            user = new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.TenantId
            }
        });
    }
}

public record LoginRequest(string Email, string TenantIdentifier);