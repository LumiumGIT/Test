namespace Lumium.Application.Features.Certificates.DTOs;

public class CreateCertificateDto
{
    public Guid? ClientId { get; set; }
    public string CertificateName { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; } = DateTime.Today;
    public DateTime ExpiryDate { get; set; } = DateTime.Today.AddYears(1);
    public string IssuedBy { get; set; } = string.Empty;
    public string? Notes { get; set; }
}