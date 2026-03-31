using Domain.Entities.Portal.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant.Public;

public class CountryRiskCategoryConfiguration : IEntityTypeConfiguration<CountryRiskCategory>
{
    public void Configure(EntityTypeBuilder<CountryRiskCategory> builder)
    {
        builder.ToTable("country_risk_categories", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Score).HasColumnName("score");
    }
}