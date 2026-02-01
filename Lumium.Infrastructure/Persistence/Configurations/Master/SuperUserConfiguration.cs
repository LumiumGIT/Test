using Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Master;

public class SuperUserConfiguration : IEntityTypeConfiguration<SuperUser>
{
    public void Configure(EntityTypeBuilder<SuperUser> builder)
    {
        builder.ToTable("super_users");
        
        builder.Property(su => su.Id).HasColumnName("id").IsRequired();
        builder.Property(su => su.Email).HasColumnName("email").HasMaxLength(255).IsRequired();
        builder.Property(su => su.PasswordHash).HasColumnName("password_hash").HasMaxLength(500).IsRequired();
        builder.Property(su => su.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
        builder.Property(su => su.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
        builder.Property(su => su.IsActive).HasColumnName("is_active").IsRequired();
        builder.Property(su => su.LastLoginAt).HasColumnName("last_login_at");
        builder.Property(su => su.CreatedAt).HasColumnName("created_at").IsRequired();
        builder.Property(su => su.UpdatedAt).HasColumnName("updated_at");
        
        builder.HasIndex(su => su.Email).IsUnique();
        builder.HasIndex(su => su.IsActive);
    }
}