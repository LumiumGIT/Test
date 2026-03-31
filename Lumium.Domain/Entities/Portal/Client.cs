using Domain.Common;
using Domain.Entities.Portal.Public;
using Domain.Enums.Clients;

namespace Domain.Entities.Portal;

public class Client : TenantEntity
{
    // Basic Info
    public string Name { get; set; } = string.Empty;
    public LegalForm LegalForm { get; set; } = LegalForm.Entrepreneur;
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

    // Status Management
    public ClientStatus Status { get; set; } = ClientStatus.Active;
    public ClientSubStatus SubStatus { get; set; } = ClientSubStatus.Standard;

    // DEPRECATED - zadržaj za backward compatibility, ali koristi Status
    public bool IsActive { get; set; } = true;

    // Flags/Checkboxes
    public bool EcoTax { get; set; }
    public bool BeneficialOwners { get; set; }
    public bool Croso { get; set; }
    public bool Pep { get; set; }
    public bool WingsTemplate { get; set; }

    // Additional
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;
    
    // Foreign Keys
    public int CountryId { get; set; }
    public int BusinessActivityId { get; set; }

    // Navigation - Public schema references
    public Country Country { get; set; } = null!;
    public BusinessActivity BusinessActivity { get; set; } = null!;

    // Navigation
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
}