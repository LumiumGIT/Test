using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class CertificateConfiguration : TenantEntityConfiguration<Certificate>
{
    public override void Configure(EntityTypeBuilder<Certificate> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("certificates");

        builder.Property(c => c.ClientId).HasColumnName("client_id");
        builder.Property(c => c.CertificateName).HasColumnName("certificate_name");
        builder.Property(c => c.CertificateNumber).HasColumnName("certificate_number");
        builder.Property(c => c.IssueDate).HasColumnName("issue_date");
        builder.Property(c => c.ExpiryDate).HasColumnName("expiry_date");
        builder.Property(c => c.RegulatoryBodyId).HasColumnName("regulatory_body_id");
        builder.Property(c => c.Notes).HasColumnName("notes");

        // Relationships
        builder.HasOne(c => c.Client)
            .WithMany(cl => cl.Certificates)
            .HasForeignKey(c => c.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}