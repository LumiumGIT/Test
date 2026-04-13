using Domain.Enums.Clients;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.ClientContacts.DTOs;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Documents.DTOs;
using Lumium.Application.Features.Public.BusinessActivities.DTOs;
using Lumium.Application.Features.Public.Countries.DTOs;

namespace Lumium.Application.Features.Clients.DTOs;

public class ClientDetailsDto
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
    public string Director { get; set; } = string.Empty;
    public ClientContactDto? PrimaryContact { get; set; }

    public bool IsActive { get; set; }
    public BusinessActivityDto? BusinessActivity { get; set; }
    public CountryDto? Country { get; set; }
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;

    public DateTime CreatedAt { get; set; }

    // Related data
    public List<ContractDto> Contracts { get; set; } = [];
    public List<CertificateDto> Certificates { get; set; } = [];
    public List<DocumentDto> Documents { get; set; } = [];
}