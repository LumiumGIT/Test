using Domain.Enums.Contracts;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Contracts.Queries;
using LumiumPortal.Web.Components.Shared;
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
            await LoadContracts();
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private Color GetStatusColor(ContractStatus status) => status switch
    {
        ContractStatus.Active => Color.Success,
        ContractStatus.Pending => Color.Warning,
        ContractStatus.Completed => Color.Info,
        ContractStatus.Cancelled => Color.Error,
        _ => Color.Default
    };

    private string GetStatusText(ContractStatus status) => status switch
    {
        ContractStatus.Active => "Aktivan",
        ContractStatus.Pending => "Na čekanju",
        ContractStatus.Completed => "Završen",
        ContractStatus.Cancelled => "Otkazan",
        _ => "Nepoznat"
    };

    private string GetTypeText(ContractType type) => type switch
    {
        ContractType.Recurring => "Ponavljajući",
        ContractType.OneTime => "Jednokratni",
        _ => "Nepoznat"
    };
}