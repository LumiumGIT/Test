using Domain.Entities.Portal;
using Domain.Entities.Portal.Public;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Common.Interfaces;

public interface IApplicationDbContext : IAsyncDisposable
{
    // Tenant-specific
    public DbSet<User> Users { get; }
    public DbSet<Client> Clients { get; }
    public DbSet<Certificate> Certificates { get; }
    public DbSet<Contract> Contracts { get; }
    public DbSet<Document> Documents { get; }

    // Shared lookup (public schema)
    public DbSet<RegulatoryBody> RegulatoryBodies { get; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<CountryRiskCategory> CountryRiskCategories { get; set; }
    public DbSet<BaRiskCategory> BaRiskCategories { get; }
    public DbSet<BusinessActivity> BusinessActivities { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    public Task SetSearchPathAsync(string schemaName);
}