using Domain.Enums;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class Clients : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    
    private List<ClientDto> _clients = [];
    
    // Filters
    private string _searchQuery = "";
    private string _legalFormFilter = "all";
    private RiskLevel? _riskFilter;
    private bool _showFilters;
    
    // Selection
    private HashSet<Guid> _selectedClients = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadClients();
    }

    private async Task LoadClients()
    {
        try
        {
            _clients = await Mediator.Send(new GetClientsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju klijenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"Error loading clients: {ex}");
        }
    }

    private IEnumerable<ClientDto> GetFilteredClients()
    {
        return _clients.Where(c =>
        {
            var matchesSearch = string.IsNullOrWhiteSpace(_searchQuery) ||
                                c.Name.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                c.Email.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                c.TaxNumber.Contains(_searchQuery, StringComparison.OrdinalIgnoreCase);

            var matchesLegalForm = _legalFormFilter == "all" || c.LegalForm == _legalFormFilter;
        
            var matchesRisk = !_riskFilter.HasValue || c.RiskLevel == _riskFilter.Value;

            return matchesSearch && matchesLegalForm && matchesRisk;
        });
    }
    
    private async Task OpenAddClientDialog()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<AddClientDialog>("Dodaj klijenta", options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadClients();
        }
    }

    private void ClearFilters()
    {
        _legalFormFilter = "all";
        _riskFilter = null;
    }

    private int GetActiveFiltersCount()
    {
        int count = 0;
        
        if (_legalFormFilter != "all") count++;
        if (_riskFilter.HasValue) count++;
        
        return count;
    }

    // Selection methods
    private bool IsAllSelected()
    {
        var filtered = GetFilteredClients().ToList();
        return filtered.Any() && filtered.All(c => _selectedClients.Contains(c.Id));
    }

    private void ToggleSelectAll()
    {
        var filtered = GetFilteredClients().ToList();
        if (IsAllSelected())
        {
            foreach (var client in filtered)
            {
                _selectedClients.Remove(client.Id);
            }
        }
        else
        {
            foreach (var client in filtered)
            {
                _selectedClients.Add(client.Id);
            }
        }
    }

    private void ToggleSelectClient(Guid id)
    {
        if (!_selectedClients.Add(id))
        {
            _selectedClients.Remove(id);
        }
    }

    // Action handlers
    private void ViewClient(Guid id)
    {
        Console.WriteLine($"View client: {id}");
    }

    // Helper methods for styling
    private Color GetRiskColor(RiskLevel risk) => risk switch
    {
        RiskLevel.Low => Color.Success,
        RiskLevel.Medium => Color.Warning,
        RiskLevel.High => Color.Error,
        _ => Color.Default
    };
}