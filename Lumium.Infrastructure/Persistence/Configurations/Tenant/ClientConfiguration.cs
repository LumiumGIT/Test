using Domain.Entities.Portal;
using Domain.Enums.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class ClientConfiguration : TenantEntityConfiguration<Client>
{
    public override void Configure(EntityTypeBuilder<Client> builder)
    {
        base.Configure(builder);

        builder.ToTable("clients");

        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.LegalForm).HasColumnName("legal_form").HasConversion<int>();
        builder.Property(c => c.TaxNumber).HasColumnName("tax_number");
        builder.Property(c => c.TaxIdentificationNumber).HasColumnName("tax_identification_number");
        builder.Property(c => c.IsPdv).HasColumnName("is_pdv");
        builder.Property(c => c.ResponsiblePerson).HasColumnName("responsible_person");
        builder.Property(c => c.BackupPerson).HasColumnName("backup_person");
        builder.Property(c => c.Address).HasColumnName("address");
        builder.Property(c => c.PhoneNumber).HasColumnName("phone_number");
        builder.Property(c => c.Director).HasColumnName("director");
        builder.Property(c => c.Email).HasColumnName("email");
        builder.Property(c => c.EcoTax).HasColumnName("eco_tax");
        builder.Property(c => c.BeneficialOwners).HasColumnName("beneficial_owners");
        builder.Property(c => c.Croso).HasColumnName("croso");
        builder.Property(c => c.Pep).HasColumnName("pep");
        builder.Property(c => c.WingsTemplate).HasColumnName("wings_template");
        builder.Property(c => c.Status).HasColumnName("status").HasDefaultValue(ClientStatus.Active)
            .HasConversion<int>();
        builder.Property(c => c.SubStatus).HasColumnName("sub_status").HasDefaultValue(ClientSubStatus.Standard)
            .HasConversion<int>();
        builder.Property(c => c.IsActive).HasColumnName("is_active");
        builder.Property(c => c.RiskLevel).HasColumnName("risk_level").HasConversion<int>();
        builder.Property(c => c.CountryId).HasColumnName("country_id");
        builder.Property(c => c.BusinessActivityId).HasColumnName("business_activity_id");

        builder.HasOne(c => c.Country).WithMany().HasForeignKey(c => c.CountryId);
        builder.HasOne(c => c.BusinessActivity).WithMany().HasForeignKey(c => c.BusinessActivityId);
    }
}