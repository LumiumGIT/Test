using Domain.Entities.Portal.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Portal.Public;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("countries", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IsoCode).HasColumnName("iso_code").HasMaxLength(2);
        builder.Property(x => x.RiskCategoryId).HasColumnName("risk_category_id");

        builder.HasIndex(x => x.IsoCode).IsUnique();

        builder.HasOne(x => x.RiskCategory).WithMany(x => x.Countries).HasForeignKey(x => x.RiskCategoryId);
    }
}