using Domain.Enums.Clients;

namespace Lumium.Application.Features.Dashboard.DTOs;

public class ClientStatusDistributionDto
{
    public ClientStatus Status { get; set; } = ClientStatus.Active;
    public int Count { get; set; }
}