using Domain.Enums;

namespace Lumium.Application.Features.Clients.DTOs;

public class CertificateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public CertificateStatus Status { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
}