using Domain.Enums;

namespace Lumium.Application.Features.Certificates.DTOs;

public class CertificateDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string CertificateName { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int RegulatoryBodyId { get; set; }
    public string RegulatoryBodyName { get; set; } = string.Empty; 
    public string? Notes { get; set; }
    public CertificateStatus Status => DaysUntilExpiry < 0
        ? CertificateStatus.Expired
        : DaysUntilExpiry <= 30
            ? CertificateStatus.AboutToExpire
            : DaysUntilExpiry <= 45
                ? CertificateStatus.ExpiringSoon
                : CertificateStatus.Valid;
    public int DaysUntilExpiry => (ExpiryDate - DateTime.Today).Days;
}