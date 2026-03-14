namespace Lumium.Application.Features.Dashboard.DTOs;

public class RecentClientDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int CertificatesCount { get; set; }
    public int ContractsCount { get; set; }
    public int DocumentsCount { get; set; }
    public string DaysAgo { get; set; } = string.Empty;
}