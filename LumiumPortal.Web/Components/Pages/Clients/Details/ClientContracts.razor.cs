using Domain.Enums;
using Domain.Enums.Contracts;
using Lumium.Application.Features.Clients.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientContracts : ComponentBase
{
    [Parameter, EditorRequired] public List<ContractDto> Contracts { get; set; } = [];

    [Parameter] public EventCallback OnAddContract { get; set; }

    private Color GetContractStatusColor(ContractStatus status)
    {
        return status switch
        {
            ContractStatus.Active => Color.Success,
            ContractStatus.Completed => Color.Default,
            ContractStatus.Pending => Color.Warning,
            ContractStatus.Cancelled => Color.Error,
            _ => Color.Default
        };
    }
}