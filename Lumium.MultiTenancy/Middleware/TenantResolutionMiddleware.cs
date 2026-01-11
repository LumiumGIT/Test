using Lumium.Infrastructure.Persistence;
using Lumium.MultiTenancy.Interfaces;
using Lumium.MultiTenancy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lumium.MultiTenancy.Middleware;

public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ITenantContext tenantContext,
        MasterDbContext masterDb)
    {
        string? tenantIdentifier = null;

        // 1. Pokušaj iz Header-a
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            tenantIdentifier = headerValue.ToString();
        }
        
        // 2. Pokušaj iz JWT claim-a (ako postoji)
        if (string.IsNullOrEmpty(tenantIdentifier) && context.User.Identity?.IsAuthenticated == true)
        {
            tenantIdentifier = context.User.FindFirst("tenant_id")?.Value;
        }

        // 3. Resolve tenant iz master database
        if (!string.IsNullOrEmpty(tenantIdentifier))
        {
            var tenant = await masterDb.Tenants
                .FirstOrDefaultAsync(t => t.Identifier == tenantIdentifier && t.IsActive);

            if (tenant != null)
            {
                ((TenantContext)tenantContext).SetTenant(
                    tenant.Id.ToString(), 
                    tenant.SchemaName);
            }
        }

        await next(context);
    }
}