using Domain.Entities.Portal.Tenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Portal.Tenant;

public class DocumentConfiguration : TenantEntityConfiguration<Document>
{
    public override void Configure(EntityTypeBuilder<Document> builder)
    {
        base.Configure(builder);

        builder.ToTable("documents");

        builder.Property(d => d.Name).HasColumnName("name");
        builder.Property(d => d.Category).HasColumnName("category").HasConversion<int>();
        builder.Property(d => d.Url).HasColumnName("url");
        builder.Property(d => d.Description).HasColumnName("description");
        builder.Property(d => d.UploadedAt).HasColumnName("uploaded_at").HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(d => d.ClientId).HasColumnName("client_id");

        // Relationships
        builder.HasOne(d => d.Client).WithMany(cl => cl.Documents).HasForeignKey(d => d.ClientId);
    }
}