using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class RegulatoryBodyConfiguration : IEntityTypeConfiguration<RegulatoryBody>
{
    public void Configure(EntityTypeBuilder<RegulatoryBody> builder)
    {
        builder.ToTable("regulatory_bodies", "public");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
    }
}