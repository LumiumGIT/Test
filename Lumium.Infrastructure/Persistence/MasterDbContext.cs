using Domain.Entities;
using Lumium.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Infrastructure.Persistence;

public class MasterDbContext(DbContextOptions<MasterDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ApplyConfigurations(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ApplyConfigurations(ModelBuilder modelBuilder)
    {
        var configurationTypes = ReflectionHelper
            .GetAllTypesImplementingOpenGenericType(
                typeof(IEntityTypeConfiguration<>),
                typeof(MasterDbContext).Assembly)
            .Where(t => !t.IsAbstract)
            .Where(t => t.Namespace?.Contains("Configurations.Master") == true);

        foreach (var configurationType in configurationTypes)
        {
            dynamic configuration = Activator.CreateInstance(configurationType)!;
            modelBuilder.ApplyConfiguration(configuration);
        }
    }
}