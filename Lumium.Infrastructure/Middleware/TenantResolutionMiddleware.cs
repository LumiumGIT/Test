using Lumium.Application.Common.Interfaces;
using Lumium.Infrastructure.MultiTenancy;
using Lumium.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Infrastructure.Middleware;

public class TenantResolutionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        ITenantContext tenantContext,
        MasterDbContext masterDb)  // ← DI injektuje ovo
    {
        string? tenantIdentifier = null;

        // 1. Header
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            tenantIdentifier = headerValue.ToString();
        }
        
        // 2. JWT claim (kasnije)
        if (string.IsNullOrEmpty(tenantIdentifier) && 
            context.User.Identity?.IsAuthenticated == true)
        {
            tenantIdentifier = context.User.FindFirst("tenant_id")?.Value;
        }

        // 3. Resolve tenant
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