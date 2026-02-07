using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class UserConfiguration : TenantEntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("users");
        
        builder.Property(e => e.Email).HasColumnName("email");
        builder.Property(e => e.PasswordHash).HasColumnName("password_hash");
        builder.Property(e => e.FirstName).HasColumnName("first_name");
        builder.Property(e => e.LastName).HasColumnName("last_name");
        builder.Property(e => e.IsActive).HasColumnName("is_active");

        builder.HasIndex(e => e.Email);
    }
}