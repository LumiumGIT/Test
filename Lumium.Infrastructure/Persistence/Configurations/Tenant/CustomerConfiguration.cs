using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class CustomerConfiguration : TenantEntityConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("customers");

        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Email).HasColumnName("email");
        builder.Property(e => e.Phone).HasColumnName("phone");
        builder.Property(e => e.Address).HasColumnName("address");
        builder.Property(e => e.IsActive).HasColumnName("is_active");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
        builder.Property(e => e.UpdatedAt).HasColumnName("updated_at");

        builder.HasIndex(e => e.Email);
        builder.HasIndex(e => e.TenantId);
    }
}