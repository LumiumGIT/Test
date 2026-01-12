using Domain.Entities;
using Lumium.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    ITenantContext tenantContext)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Postavi default schema na tenant schema
        if (!string.IsNullOrEmpty(tenantContext.SchemaName))
        {
            modelBuilder.HasDefaultSchema(tenantContext.SchemaName);
        }
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TenantId).HasColumnName("tenant_id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            
            // Index na email za brže pretraživanje
            entity.HasIndex(e => e.Email);
        });

        base.OnModelCreating(modelBuilder);
    }
}