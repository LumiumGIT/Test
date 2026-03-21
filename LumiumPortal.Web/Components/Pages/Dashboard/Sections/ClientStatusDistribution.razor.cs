using Domain.Enums.Clients;
using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class ClientStatusDistribution : ComponentBase
{
    [Parameter] public List<ClientStatusDistributionDto> StatusDistribution { get; set; } = [];

    private ClientStatus? _expandedStatus;

    private void ToggleStatus(ClientStatus status)
    {
        _expandedStatus = _expandedStatus == status ? null : status;
    }
    
    private string GetColorHex(Color color)
    {
        return color switch
        {
            Color.Success => "var(--mud-palette-success)",
            Color.Info => "var(--mud-palette-info)",
            Color.Primary => "var(--mud-palette-primary)",
            Color.Warning => "var(--mud-palette-warning)",
            Color.Error => "var(--mud-palette-error)",
            _ => "var(--mud-palette-text-secondary)"
        };
    }
}