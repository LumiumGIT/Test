using Dapper;
using Domain.Entities;
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
    IPasswordHasher passwordHasher,
    ITenantContext tenantContext)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Nađi tenant
        var tenant = await masterContext.Tenants
            .FirstOrDefaultAsync(t => 
                t.Identifier == request.TenantIdentifier && t.IsActive);

        if (tenant == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // 2. Setuj tenant context (pre kreiranja ApplicationDbContext-a!)
        ((TenantContext)tenantContext).SetTenant(
            tenant.Id.ToString(), 
            tenant.SchemaName);

        // 3. ⭐ Dapper - Najčistije rešenje
        await using var connection = appContext.Database.GetDbConnection();
    
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        // Setuj search_path
        await connection.ExecuteAsync($"SET search_path TO {tenant.SchemaName}");

        // Query user sa Dapper-om
        var user = await connection.QuerySingleOrDefaultAsync<User>(
            "SELECT * FROM users WHERE email = @Email AND is_active = true",
            new { Email = request.Email });

        if (user == null)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // 4. Verify password
        if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        // 5. Generiši JWT token
        var token = jwtService.GenerateToken(
            user.Id, 
            user.Email, 
            user.TenantId);

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

public record LoginRequest(string Email, string TenantIdentifier, string Password);