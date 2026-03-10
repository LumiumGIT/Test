using Domain.Enums.Contracts;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using LumiumPortal.Web.Components.Pages.Contracts;
using LumiumPortal.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientContracts : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    
    [Parameter, EditorRequired] public List<ContractDto> Contracts { get; set; } = [];
    
    private async Task OpenAddContractDialog()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<AddContractDialog>("Dodaj ugovor", options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
           // await LoadContracts();
        }
    }

    private async Task OpenDeleteDialog(ContractDto contract)
    {
        var parameters = new DialogParameters
        {
            { nameof(ConfirmDialog.Message), $"Da li ste sigurni da želite da obrišete ugovor '{contract.ContractNumber}'?" },
            { nameof(ConfirmDialog.ConfirmText), "Obriši" },
            { nameof(ConfirmDialog.ConfirmColor), Color.Error }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Potvrda brisanja", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await DeleteContract(contract.Id);
        }
    }
    
    private async Task DeleteContract(Guid id)
    {
        var result = await Mediator.Send(new DeleteContractCommand(id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

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