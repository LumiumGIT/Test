using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task SetSearchPathAsync(string schemaName);
}