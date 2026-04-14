using Domain.Enums.Contracts;
using Lumium.Application.Common.Extensions;

namespace Lumium.Application.Features.Contracts.DTOs;

public class ContractDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    public string ContractNumber { get; set; } = string.Empty;
    public decimal MonthlyFee { get; set; }
    public string? Notes { get; set; }

    public ContractStatus Status { get; set; }
    public ContractType Type { get; set; }
    public ContractDuration Duration { get; set; }
    public ContractKind Kind { get; set; }
    
    public Guid? ParentContractId { get; set; }
    public List<ContractDto> Annexes { get; set; } = [];

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public bool IsHistoryRow { get; set; }

    // Last annex or main contract
    public ContractDto ActiveVersion => Annexes.Count > 0
        ? Annexes.OrderByDescending(a => a.CreatedAt).First()
        : this;
    
    public string DurationDisplay => Duration == ContractDuration.Indefinite
        ? ContractDuration.Indefinite.GetDescription()
        : EndDate?.ToString("dd.MM.yyyy") ?? "-";

    public bool IsExpired => Duration == ContractDuration.Fixed
                             && EndDate.HasValue
                             && EndDate.Value < DateTime.Today;

    public int? DaysUntilExpiry => Duration == ContractDuration.Fixed && EndDate.HasValue
        ? (EndDate.Value - DateTime.Today).Days
        : null;
}