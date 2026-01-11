using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Infrastructure.Persistence;

public class MasterDbContext(DbContextOptions<MasterDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("tenants");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Identifier).HasColumnName("identifier");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SchemaName).HasColumnName("schema_name");
            entity.Property(e => e.ConnectionString).HasColumnName("connection_string");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        base.OnModelCreating(modelBuilder);
    }
}