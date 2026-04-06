using Domain.Enums.Clients;

namespace Lumium.Application.Features.Clients.DTOs;

public class ClientFormDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LegalForm LegalForm { get; set; } = LegalForm.Entrepreneur;
    public string TaxNumber { get; set; } = string.Empty;
    public string TaxIdentificationNumber { get; set; } = string.Empty;
    public bool IsPdv { get; set; }

    public string ResponsiblePerson { get; set; } = string.Empty;
    public string BackupPerson { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public bool EcoTax { get; set; }
    public bool BeneficialOwners { get; set; }
    public bool Croso { get; set; }
    public bool Pep { get; set; }
    public bool WingsTemplate { get; set; }
    public bool IsActive { get; set; }

    public int CountryId { get; set; } = 1;
    public int BusinessActivityId { get; set; } = 1;
    
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;
}