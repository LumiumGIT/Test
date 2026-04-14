using Domain.Enums.Contracts;
using Lumium.Application.Features.Contracts.Commands;
using Lumium.Application.Features.Contracts.DTOs;
using Lumium.Application.Features.Contracts.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Contracts;

public partial class Contracts : SecureComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private List<ContractDto> _contracts = [];
    private List<ContractDto> _flatContracts = [];
    private HashSet<Guid> _expandedRows = [];
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
            BuildFlatList();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju ugovora: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load contracts failed: {ex}");
        }
    }

    private void BuildFlatList()
    {
        _flatContracts = [];

        foreach (var contract in _contracts)
        {
            _flatContracts.Add(contract);

            if (_expandedRows.Contains(contract.Id) && contract.Annexes.Count > 0)
            {
                // Osnovni ugovor kao history red — može se menjati, ne i brisati
                var mainHistoryRow = new ContractDto
                {
                    Id = contract.Id,
                    ClientId = contract.ClientId,
                    ClientName = contract.ClientName,
                    ContractNumber = contract.ContractNumber,
                    MonthlyFee = contract.MonthlyFee,
                    Notes = contract.Notes,
                    Status = contract.Status,
                    Type = contract.Type,
                    Duration = contract.Duration,
                    Kind = contract.Kind,
                    StartDate = contract.StartDate,
                    EndDate = contract.EndDate,
                    CreatedAt = contract.CreatedAt,
                    IsHistoryRow = true
                };

                _flatContracts.Add(mainHistoryRow);

                // Aneksi sortirani od najstarijeg
                foreach (var annex in contract.Annexes.OrderBy(a => a.CreatedAt))
                {
                    _flatContracts.Add(annex);
                }
            }
        }
    }

    private void ToggleRow(Guid contractId)
    {
        if (_expandedRows.Contains(contractId))
            _expandedRows.Remove(contractId);
        else
            _expandedRows.Add(contractId);

        BuildFlatList();
        StateHasChanged();
    }

    private bool IsExpanded(Guid contractId) => _expandedRows.Contains(contractId);
    private bool IsAnnex(ContractDto contract) => contract.Kind == ContractKind.Annex;

    private string GetRowStyle(ContractDto contract, int index)
    {
        if (contract.IsHistoryRow)
            return "background-color: var(--mud-palette-table-striped); border-left: 3px solid var(--mud-palette-grey-default); opacity: 0.85;";
        if (IsAnnex(contract))
            return "background-color: var(--mud-palette-table-striped); border-left: 3px solid var(--mud-palette-primary);";
        return string.Empty;
    }

    private async Task AddContract()
    {
        if (await DialogService.ShowAddContractDialog())
            await LoadContracts();
    }

    private async Task AddAnnex(ContractDto contract)
    {
        if (await DialogService.ShowAddAnnexDialog(contract))
            await LoadContracts();
    }

    private async Task EditContract(ContractDto contract)
    {
        // History row ili aneks — menjamo direktno
        // Glavni red — menjamo ActiveVersion (poslednji aneks ili sam sebe)
        var toEdit = (contract.IsHistoryRow || IsAnnex(contract))
            ? contract
            : contract.ActiveVersion;

        if (await DialogService.ShowEditContractDialog(toEdit))
            await LoadContracts();
    }

    private async Task DeleteContract(ContractDto contract)
    {
        // Brišemo direktno aneks
        if (!await DialogService.ShowDeleteContractConfirmation(contract.ContractNumber))
            return;

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