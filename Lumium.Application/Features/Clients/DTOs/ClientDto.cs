using Domain.Enums;

namespace Lumium.Application.Features.Clients.DTOs;

public class ClientDto
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
    public bool BusinessActivity { get; set; }
    
    public string Country { get; set; } = string.Empty;
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;
    
    public DateTime CreatedAt { get; set; }
}