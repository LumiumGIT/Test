using Lumium.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ITenantContext tenantContext)
    : DbContext(options)
{
    // ← Ovde ćeš dodavati DbSet-ove (Customer, Order, itd)

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Postavi default schema na tenant schema
        if (!string.IsNullOrEmpty(tenantContext.SchemaName))
        {
            modelBuilder.HasDefaultSchema(tenantContext.SchemaName);
        }

        base.OnModelCreating(modelBuilder);
    }
}