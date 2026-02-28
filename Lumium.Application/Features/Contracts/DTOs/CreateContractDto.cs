using Domain.Enums.Contracts;

namespace Lumium.Application.Features.Contracts.DTOs;

public class CreateContractDto
{
    public Guid? ClientId { get; set; }
    
    public string ContractNumber { get; set; } = string.Empty;
    public decimal MonthlyFee { get; set; }
    public string? Notes { get; set; }
    
    public ContractStatus Status { get; set; } = ContractStatus.Active;
    public ContractType Type { get; set; } = ContractType.Recurring;
    public ContractDuration Duration { get; set; } = ContractDuration.Fixed;
    
    public DateTime StartDate { get; set; } = DateTime.Today;
    public DateTime? EndDate { get; set; } = DateTime.Today.AddYears(1);
}