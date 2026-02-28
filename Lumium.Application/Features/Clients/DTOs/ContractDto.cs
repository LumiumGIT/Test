using Domain.Enums;
using Domain.Enums.Contracts;

namespace Lumium.Application.Features.Clients.DTOs;

public class ContractDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ContractType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Value { get; set; }
    public ContractStatus Status { get; set; }
}