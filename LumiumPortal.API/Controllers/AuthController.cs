using Lumium.Application.Common.Interfaces;
using Lumium.Contracts;
using Lumium.Infrastructure.MultiTenancy;
using Lumium.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumiumPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    MasterDbContext masterContext,
    ApplicationDbContext appContext,
    IJwtService jwtService,
    IPasswordHasher passwordHasher,
    ITenantContext tenantContext)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var tenant = await masterContext.Tenants
            .FirstOrDefaultAsync(t =>
                t.Identifier == request.TenantIdentifier && t.IsActive);

        if (tenant == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        ((TenantContext)tenantContext).SetTenant(tenant.Id, tenant.SchemaName);

        await appContext.SetSearchPathAsync(tenant.SchemaName);

        var user = await appContext.Users
            .Where(u => u.Email == request.Email && u.IsActive)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = jwtService.GenerateToken(
            user.Id,
            user.Email,
            user.TenantId.ToString(),
            tenant.SchemaName,
            user.FirstName,
            user.LastName);

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

    [HttpPost("validate-company")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateCompany([FromBody] ValidateCompanyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyName))
        {
            return BadRequest(new { message = "Company name is required" });
        }

        var tenant = await masterContext.Tenants
            .Where(t => t.IsActive)
            .FirstOrDefaultAsync(t => EF.Functions.ILike(t.Name, request.CompanyName));

        if (tenant == null)
        {
            return NotFound(new { message = "Company not found" });
        }

        return Ok(new
        {
            identifier = tenant.Identifier,
            name = tenant.Name
        });
    }

    #region Admin

    [HttpPost("admin/login")]
    [AllowAnonymous]
    public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
    {
        var superUser = await masterContext.SuperUsers
            .FirstOrDefaultAsync(su => su.Email == request.Email && su.IsActive);

        if (superUser == null)
        {
            return Unauthorized(new { message = "Nevalidni pristupni podaci" });
        }

        if (!passwordHasher.VerifyPassword(request.Password, superUser.PasswordHash))
        {
            return Unauthorized(new { message = "Nevalidni pristupni podaci" });
        }

        superUser.LastLoginAt = DateTime.UtcNow;
        await masterContext.SaveChangesAsync();

        var token = jwtService.GenerateToken(
            superUser.Id,
            superUser.Email,
            Guid.Empty.ToString(),
            "public",
            superUser.FirstName,
            superUser.LastName);

        return Ok(new
        {
            token,
            user = new
            {
                superUser.Id,
                superUser.Email,
                superUser.FirstName,
                superUser.LastName
            }
        });
    }

    #endregion
}