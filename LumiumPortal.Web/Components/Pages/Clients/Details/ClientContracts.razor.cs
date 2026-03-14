using Lumium.Application.Common.Models;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Contracts.Queries;
using LumiumPortal.Web.Components.Pages.Contracts;
using LumiumPortal.Web.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientContracts : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    
    [Parameter] public Guid ClientId { get; set; }
    
    private List<ContractDto> _contracts = [];
    
    protected override async Task OnInitializedAsync()
    {
        await LoadContracts();
        
        await base.OnInitializedAsync();
    }
    
    private async Task OpenAddContractDialog()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddContractDialog.ClientId), ClientId}
        };
        
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<AddContractDialog>("Dodaj ugovor", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
           await LoadContracts();
        }
    }
    
    private async Task LoadContracts()
    {
        try
        {
           _contracts = await Mediator.Send(new GetContractsByClientQuery(ClientId));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju ugovora: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load contracts failed: {ex}");
        }
    }

    private async Task DeleteContract(ContractDto contract)
    {
        var confirmed = await DialogHelpers.ShowConfirmDialog(
            DialogService,
            message: $"Da li ste sigurni da želite da obrišete ugovor '{contract.ContractNumber}'?",
            title: "Potvrda brisanja",
            confirmText: "Obriši",
            confirmColor: Color.Error
        );

        if (confirmed)
        {
            var result = await Mediator.Send(new DeleteContractCommand(contract.Id));
            
            await HandleResult(result);
        }
    }
    
    private async Task HandleResult(Result result)
    {

        if (result.IsSuccess)
        {
            await LoadContracts();
            Snackbar.Add(result.Message, Severity.Success);
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }
}