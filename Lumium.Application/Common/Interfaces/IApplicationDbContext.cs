using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Common.Interfaces;

public interface IApplicationDbContext : IAsyncDisposable
{
    // Tenant-specific
    DbSet<User> Users { get; }
    DbSet<Client> Clients { get; }
    DbSet<Certificate> Certificates { get; }
    
    // Shared lookup (public schema)
    DbSet<RegulatoryBody> RegulatoryBodies { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task SetSearchPathAsync(string schemaName);
}