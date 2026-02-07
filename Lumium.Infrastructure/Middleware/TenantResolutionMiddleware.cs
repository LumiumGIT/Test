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
        MasterDbContext masterDb)
    {
        Guid? tenantId = null;

        // 1. JWT Claim (sadrži tenant ID kao GUID)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var claimValue = context.User.FindFirst("tenant_id")?.Value;
            
            if (!string.IsNullOrEmpty(claimValue) && Guid.TryParse(claimValue, out var parsedId))
            {
                tenantId = parsedId;
            }
        }

        // 2. Subdomain (sadrži identifier kao string)
        if (!tenantId.HasValue)
        {
            var host = context.Request.Host.Host;
            var parts = host.Split('.');

            if (parts.Length > 2)
            {
                var subdomain = parts[0];
                var tenant = await masterDb.Tenants
                    .FirstOrDefaultAsync(t => t.Identifier == subdomain && t.IsActive);

                if (tenant != null)
                {
                    tenantId = tenant.Id;
                }
            }
        }

        // 3. X-Tenant-Id Header (može biti identifier ili id)
        if (!tenantId.HasValue)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
            {
                if (Guid.TryParse(headerValue.ToString(), out var parsedId))
                {
                    tenantId = parsedId;
                }
                else
                {
                    // Ako nije GUID, probaj kao identifier
                    var tenant = await masterDb.Tenants
                        .FirstOrDefaultAsync(t => t.Identifier == headerValue.ToString() && t.IsActive);
                
                    if (tenant != null)
                    {
                        tenantId = tenant.Id;
                    }
                }
            }
        }

        // 4. Resolve tenant po ID-u
        if (tenantId.HasValue)
        {
            var tenant = await masterDb.Tenants
                .FirstOrDefaultAsync(t => t.Id == tenantId.Value && t.IsActive);

            if (tenant != null)
            {
                ((TenantContext)tenantContext).SetTenant(tenant.Id, tenant.SchemaName);
            }
        }

        await next(context);
    }
}