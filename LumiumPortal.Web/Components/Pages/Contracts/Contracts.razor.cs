using Domain.Enums.Contracts;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Contracts.Queries;
using LumiumPortal.Web.Helpers;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Contracts;

public partial class Contracts : SecureComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private List<ContractDto> _contracts = [];
    private bool _isLoading = true;

    private int ActiveCount => _contracts.Count(c => c.Status == ContractStatus.Active);
    private int PendingCount => _contracts.Count(c => c.Status == ContractStatus.Pending);
    private int CompletedCount => _contracts.Count(c => c.Status == ContractStatus.Completed);
    private int CancelledCount => _contracts.Count(c => c.Status == ContractStatus.Cancelled);

    protected override async Task OnSecureInitializedAsync()
    {
        _isLoading = true;
        await LoadContracts();
        _isLoading = false;
    }

    private async Task LoadContracts()
    {
        try
        {
            _contracts = await Mediator.Send(new GetContractsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju ugovora: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load contracts failed: {ex}");
        }
    }

    private async Task AddContract()
    {
        if (await DialogService.ShowAddContractDialog())
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
        if (await DialogService.ShowDeleteCertificateConfirmation(contract.ContractNumber))
        {
            await HandleDeleteCertificate(contract.Id);
        }
    }
    
    private async Task HandleDeleteCertificate(Guid id)
    {
        var result = await Mediator.Send(new DeleteContractCommand(id));

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