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
        string? tenantId = null;

        // 1. JWT Claim (sadrži tenant ID kao GUID)
        if (context.User.Identity?.IsAuthenticated == true)
        {
            tenantId = context.User.FindFirst("tenant_id")?.Value;
        }
    
        // 2. Subdomain (sadrži identifier kao string)
        if (string.IsNullOrEmpty(tenantId))
        {
            var host = context.Request.Host.Host;
            var parts = host.Split('.');
        
            if (parts.Length > 2) // subdomain.lumium.com
            {
                var subdomain = parts[0];
                var tenant = await masterDb.Tenants
                    .FirstOrDefaultAsync(t => t.Identifier == subdomain && t.IsActive);
            
                if (tenant != null)
                {
                    tenantId = tenant.Id.ToString();
                }
            }
        }
    
        // 3. X-Tenant-Id Header (može biti identifier ili id)
        if (string.IsNullOrEmpty(tenantId))
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
            {
                var headerTenant = headerValue.ToString();
            
                // Probaj kao identifier prvo, pa kao ID
                var tenant = await masterDb.Tenants
                    .FirstOrDefaultAsync(t => 
                        (t.Identifier == headerTenant || t.Id.ToString() == headerTenant) 
                        && t.IsActive);
            
                if (tenant != null)
                {
                    tenantId = tenant.Id.ToString();
                }
            }
        }

        // 4. Resolve tenant po ID-u
        if (!string.IsNullOrEmpty(tenantId))
        {
            if (Guid.TryParse(tenantId, out var tenantGuid))
            {
                var tenant = await masterDb.Tenants
                    .FirstOrDefaultAsync(t => t.Id == tenantGuid && t.IsActive);
                
                ((TenantContext)tenantContext).SetTenant(
                    tenant.Id.ToString(), 
                    tenant.SchemaName);
            }
        }

        await next(context);
    }
}