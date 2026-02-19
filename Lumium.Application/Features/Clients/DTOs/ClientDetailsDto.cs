using Domain.Enums;
using Lumium.Application.Features.Certificates.DTOs;

namespace Lumium.Application.Features.Clients.DTOs;

public class ClientDetailsDto
{
    // Osnovni podaci (iz ClientDto)
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public LegalForm LegalForm { get; set; }
    public string TaxNumber { get; set; } = string.Empty;
    public string TaxIdentificationNumber { get; set; } = string.Empty;
    public bool IsPdv { get; set; }
    public string ResponsiblePerson { get; set; } = string.Empty;
    public string BackupPerson { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Director { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public RiskLevel RiskLevel { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Dodatni podaci za Details stranicu
    public string Industry { get; set; } = string.Empty;
    public string CompanySize { get; set; } = string.Empty;
    public decimal MonthlyFee { get; set; }
    public string BillingContact { get; set; } = string.Empty;
    public string AssignedTo { get; set; } = string.Empty;
    
    // Related data
    public List<ContractDto> Contracts { get; set; } = [];
    public List<CertificateDto> Certificates { get; set; } = [];
    public List<DocumentDto> Documents { get; set; } = [];
}