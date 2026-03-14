using Domain.Enums.Clients;

namespace Lumium.Application.Features.Dashboard.DTOs;

public class ClientSubStatusDistributionDto
{
    public ClientSubStatus SubStatus { get; set; } = ClientSubStatus.Standard;
    public int Count { get; set; }
}