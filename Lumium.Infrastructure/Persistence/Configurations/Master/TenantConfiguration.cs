using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Master;

public class TenantConfiguration : IEntityTypeConfiguration<Domain.Entities.Tenant>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Tenant> builder)
    {
        builder.ToTable("tenants");

        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Identifier).HasColumnName("identifier");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.SchemaName).HasColumnName("schema_name");
        builder.Property(e => e.ConnectionString).HasColumnName("connection_string");
        builder.Property(e => e.IsActive).HasColumnName("is_active");
        builder.Property(e => e.CreatedAt).HasColumnName("created_at");
    }
}