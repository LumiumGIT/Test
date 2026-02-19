using Domain.Common;

namespace Domain.Entities.Portal;

public class Certificate : TenantEntity
{
    public Guid ClientId { get; set; }
    public string CertificateName { get; set; } = string.Empty;
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public string? Notes { get; set; }
    
    // Navigation
    public Client Client { get; set; } = null!;
}