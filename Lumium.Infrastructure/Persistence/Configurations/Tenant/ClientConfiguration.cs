using Domain.Entities.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lumium.Infrastructure.Persistence.Configurations.Tenant;

public class ClientConfiguration : TenantEntityConfiguration<Client>
{
    public override void Configure(EntityTypeBuilder<Client> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("clients");

        builder.Property(c => c.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.LegalForm)
            .HasColumnName("legal_form")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.TaxNumber)
            .HasColumnName("tax_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.TaxIdentificationNumber)
            .HasColumnName("tax_identification_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.IsPdv)
            .HasColumnName("is_pdv")
            .IsRequired();

        builder.Property(c => c.ResponsiblePerson)
            .HasColumnName("responsible_person")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.BackupPerson)
            .HasColumnName("backup_person")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Address)
            .HasColumnName("address")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(c => c.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Director)
            .HasColumnName("director")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnName("email")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.EcoTax)
            .HasColumnName("eco_tax")
            .IsRequired();

        builder.Property(c => c.BeneficialOwners)
            .HasColumnName("beneficial_owners")
            .IsRequired();

        builder.Property(c => c.Croso)
            .HasColumnName("croso")
            .IsRequired();

        builder.Property(c => c.Pep)
            .HasColumnName("pep")
            .IsRequired();

        builder.Property(c => c.WingsTemplate)
            .HasColumnName("wings_template")
            .IsRequired();

        builder.Property(c => c.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(c => c.BusinessActivity)
            .HasColumnName("business_activity")
            .IsRequired();

        builder.Property(c => c.Country)
            .HasColumnName("country")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.RiskLevel)
            .HasColumnName("risk_level")
            .HasMaxLength(50)
            .IsRequired();
    }
}