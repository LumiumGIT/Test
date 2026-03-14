namespace Lumium.Application.Features.Dashboard.DTOs;

public class DashboardDataDto
{
    public DashboardAlertsDto Alerts { get; set; } = new();
    public DashboardStatsDto Stats { get; set; } = new();
    public List<UpcomingDeadlineDto> UpcomingDeadlines { get; set; } = [];
}