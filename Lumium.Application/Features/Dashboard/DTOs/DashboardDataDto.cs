namespace Lumium.Application.Features.Dashboard.DTOs;

public class DashboardDataDto
{
    public DashboardAlertsDto Alerts { get; set; } = new();
    public DashboardStatsDto Stats { get; set; } = new();
    public List<UpcomingDeadlineDto> UpcomingDeadlines { get; set; } = [];
    public List<RecentClientDto> RecentClients { get; set; } = [];
    public List<ClientAcquisitionDto> AcquisitionsThisYear { get; set; } = [];
    public List<ClientStatusDistributionDto> ClientsByStatus { get; set; } = [];
}