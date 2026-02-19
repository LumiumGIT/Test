using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Common.Interfaces;

public interface IApplicationDbContext : IAsyncDisposable
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Client> Clients { get; }
    DbSet<Certificate> Certificates { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task SetSearchPathAsync(string schemaName);
}