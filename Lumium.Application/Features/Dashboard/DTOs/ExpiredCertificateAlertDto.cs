namespace Lumium.Application.Features.Dashboard.DTOs;

public class ExpiredCertificateAlertDto
{
    public Guid CertificateId { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string CertificateName { get; set; } = string.Empty;
    public DateTime ExpiryDate { get; set; }
    public int DaysExpired { get; set; }
}