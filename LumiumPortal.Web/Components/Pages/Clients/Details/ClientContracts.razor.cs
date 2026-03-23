using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Contracts.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
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

    private async Task AddContract()
    {
        if (await DialogService.ShowAddContractDialog(ClientId))
        {
            await LoadContracts();
        }
    }
    
    private async Task EditContract(ContractDto contract)
    {
        if (await DialogService.ShowEditContractDialog(contract))
        {
            await LoadContracts();
        }
    }

    private async Task DeleteContract(ContractDto contract)
    {
        if (!await DialogService.ShowDeleteContractConfirmation(contract.ContractNumber))
        {
            return;
        }

        var result = await Mediator.Send(new DeleteContractCommand(contract.Id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
            await LoadContracts();
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }
}