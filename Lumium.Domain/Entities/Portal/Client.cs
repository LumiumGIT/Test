using Domain.Common;

namespace Domain.Entities.Portal;

public class Client : TenantEntity
{
    // Basic Info
    public string Name { get; set; } = string.Empty;
    public string LegalForm { get; set; } = string.Empty;
    public string TaxNumber { get; set; } = string.Empty;
    public string TaxIdentificationNumber { get; set; } = string.Empty;
    public bool IsPdv { get; set; }
    
    // Responsible Persons
    public string ResponsiblePerson { get; set; } = string.Empty;
    public string BackupPerson { get; set; } = string.Empty;
    
    // Contact Info
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // Flags/Checkboxes
    public bool EcoTax { get; set; }
    public bool BeneficialOwners { get; set; }
    public bool Croso { get; set; }
    public bool Pep { get; set; }
    public bool WingsTemplate { get; set; }
    public bool IsActive { get; set; }
    public bool BusinessActivity { get; set; }
    
    // Additional
    public string Country { get; set; } = string.Empty;
    public string RiskLevel { get; set; } = string.Empty;
}