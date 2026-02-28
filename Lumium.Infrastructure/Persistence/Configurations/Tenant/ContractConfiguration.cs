using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class ContractConfiguration : TenantEntityConfiguration<Contract>
{
    public override void Configure(EntityTypeBuilder<Contract> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("contracts");

        builder.Property(c => c.ContractNumber).HasColumnName("contract_number");
        builder.Property(c => c.MonthlyFee).HasColumnName("monthly_fee");
        builder.Property(c => c.Notes).HasColumnName("notes");
        builder.Property(c => c.Status).HasColumnName("status").HasConversion<int>();
        builder.Property(c => c.Type).HasColumnName("type").HasConversion<int>();
        builder.Property(c => c.Duration).HasColumnName("duration").HasConversion<int>();
        builder.Property(c => c.StartDate).HasColumnName("start_date");
        builder.Property(c => c.EndDate).HasColumnName("end_date");
        builder.Property(c => c.ClientId).HasColumnName("client_id");
        
        builder.HasOne(c => c.Client).WithMany().HasForeignKey(c => c.ClientId);
    }
}