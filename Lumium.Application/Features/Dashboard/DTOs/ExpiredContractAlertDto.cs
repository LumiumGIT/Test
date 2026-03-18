namespace Lumium.Application.Features.Dashboard.DTOs;

public class ExpiredContractAlertDto
{
    public Guid ContractId { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime? EndDate { get; set; }
    public int DaysExpired { get; set; }
}