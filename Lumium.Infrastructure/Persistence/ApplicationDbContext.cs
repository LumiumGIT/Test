using System.Data;
using System.Linq.Expressions;
using Domain.Common;
using Domain.Entities.Portal;
using Lumium.Application.Common.Interfaces;
using Lumium.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Lumium.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantContext tenantContext)
    : DbContext(options), IApplicationDbContext
{
    // Tenant-specific
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<Certificate> Certificates { get; set; } = null!;
    
    // Shared lookup (public schema)
    public DbSet<RegulatoryBody> RegulatoryBodies { get; set; } = null!;

    public string GetTenantId()
    {
        return tenantContext.TenantId.ToString();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ReplaceService<IModelCacheKeyFactory, DynamicModelCacheKeyFactory>();
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyConfigurations(modelBuilder);
        ApplyGlobalQueryFilters(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        var configurationTypes = ReflectionHelper
            .GetAllTypesImplementingOpenGenericType(
                typeof(IEntityTypeConfiguration<>),
                typeof(ApplicationDbContext).Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Namespace?.Contains("Configurations.Tenant") == true);

        foreach (var configurationType in configurationTypes)
        {
            dynamic configuration = Activator.CreateInstance(configurationType)!;
            modelBuilder.ApplyConfiguration(configuration);
        }
    }

    private void ApplyGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(TenantEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            var property = Expression.Property(parameter, nameof(TenantEntity.TenantId));
            var tenantId = Expression.Property(
                Expression.Constant(tenantContext),
                nameof(ITenantContext.TenantId));
            var body = Expression.Equal(property, tenantId);
            var lambda = Expression.Lambda(body, parameter);

            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }

    public async Task SetSearchPathAsync(string schemaName)
    {
        if (string.IsNullOrEmpty(schemaName))
            return;

        var connection = Database.GetDbConnection();

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = $"SET search_path TO {schemaName}";
        await command.ExecuteNonQueryAsync();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<TenantEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.TenantId == Guid.Empty)
            {
                if (tenantContext.TenantId == Guid.Empty)
                    throw new InvalidOperationException("TenantId nije setovan u tenant context-u");

                entry.Entity.TenantId = tenantContext.TenantId;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}