using Domain.Entities.Portal.Public;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Portal.Public;

public class BusinessActivityConfiguration : IEntityTypeConfiguration<BusinessActivity>
{
    public void Configure(EntityTypeBuilder<BusinessActivity> builder)
    {
        builder.ToTable("business_activities", "public");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Valid).HasColumnName("valid");
        builder.Property(x => x.ValidTo).HasColumnName("valid_to");
        builder.Property(x => x.RiskCategoryId).HasColumnName("risk_category_id");

        builder.HasOne(x => x.RiskCategory).WithMany(x => x.BusinessActivities).HasForeignKey(x => x.RiskCategoryId);
    }
}