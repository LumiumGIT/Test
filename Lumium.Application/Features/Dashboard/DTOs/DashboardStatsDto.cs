namespace Lumium.Application.Features.Dashboard.DTOs;

public class DashboardStatsDto
{
    public int TotalClients { get; set; }
    public int NewClientsThisMonth { get; set; }
    public int NewClientsThisYear { get; set; }
    
    public int TotalCertificates { get; set; }
    public int CertificatesExpiringSoon { get; set; }
    
    public int TotalContracts { get; set; }
    public int ActiveContracts { get; set; }
    
    public int TotalDocuments { get; set; }
    public int DocumentsThisWeek { get; set; }
}