using Domain.Common;
using Domain.Enums.Contracts;

namespace Domain.Entities.Portal;

public class Contract : TenantEntity
{
    public Guid ClientId { get; set; }
    
    public string ContractNumber { get; set; } = string.Empty;
    public decimal MonthlyFee { get; set; }
    public string? Notes { get; set; }
    
    public ContractStatus Status { get; set; }
    public ContractType Type { get; set; }
    public ContractDuration Duration { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    // Navigation
    public Client Client { get; set; } = null!;
}