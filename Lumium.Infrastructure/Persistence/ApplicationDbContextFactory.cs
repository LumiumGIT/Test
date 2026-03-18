using Lumium.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Infrastructure.Persistence;

public class ApplicationDbContextFactory(IDbContextFactory<ApplicationDbContext> factory, ITenantContext tenantContext)
    : IApplicationDbContextFactory
{
    public async Task<IApplicationDbContext> CreateDbContextAsync(
        CancellationToken cancellationToken = default)
    {
        var context = await factory.CreateDbContextAsync(cancellationToken);

        if (!string.IsNullOrEmpty(tenantContext.SchemaName))
        {
            await context.SetSearchPathAsync(tenantContext.SchemaName);
        }

        return context;
    }
}