using Domain.Enums.Contracts;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Contracts.Queries;
using LumiumPortal.Web.Helpers;
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
            await LoadContracts();
        }
    }

    private async Task DeleteContract(ContractDto contract)
    {
        var confirmed = await DialogHelpers.ShowConfirmDialog(
            DialogService,
            $"Da li ste sigurni da želite da obrišete ugovor '{contract.ContractNumber}'?",
            "Potvrda brisanja",
            "Obriši",
            Color.Error
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