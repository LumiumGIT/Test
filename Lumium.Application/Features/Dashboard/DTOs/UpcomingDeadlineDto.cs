using Domain.Enums.Shared;

namespace Lumium.Application.Features.Dashboard.DTOs;

public class UpcomingDeadlineDto
{
    public DateTime Date { get; set; }
    public DeadlineType Type { get; set; } 
    public Guid ItemId { get; set; }
    public Guid ClientId { get; set; } 
    public string Title { get; set; } = string.Empty;
    public string ClientName { get; set; } = string.Empty;
    public int DaysRemaining { get; set; }
    public DeadlineStatus Status { get; set; }
}