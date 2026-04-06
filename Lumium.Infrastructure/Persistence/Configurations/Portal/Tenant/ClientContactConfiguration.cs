using Domain.Entities.Portal.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Portal.Tenant;

public class ClientContactConfiguration : TenantEntityConfiguration<ClientContact>
{
    public override void Configure(EntityTypeBuilder<ClientContact> builder)
    {
        base.Configure(builder);

        builder.ToTable("client_contacts");

        builder.Property(x => x.ClientId).HasColumnName("client_id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Phone).HasColumnName("phone");
        builder.Property(x => x.Email).HasColumnName("email");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.Type).HasColumnName("type").HasConversion<int>();
        builder.Property(x => x.UpdatedBy).HasColumnName("up_by");

        builder.HasOne(x => x.Client)
            .WithMany(c => c.Contacts)
            .HasForeignKey(x => x.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.UpdatedByUser)
            .WithMany()
            .HasForeignKey(x => x.UpdatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}