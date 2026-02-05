using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<Client> Clients { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task SetSearchPathAsync(string schemaName);
}