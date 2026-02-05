using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages;

public partial class Clients : ComponentBase
{
    private List<ClientDto> _clients = new();
    private bool _isLoading = true;
    
    // Filters
    private string _searchQuery = "";
    private string _legalFormFilter = "all";
    private string _riskFilter = "all";
    private bool _showFilters = false;
    
    // Selection
    private HashSet<Guid> _selectedClients = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadClients();
    }

    private async Task LoadClients()
    {
        try
        {
            _isLoading = true;
            _clients = await Mediator.Send(new GetClientsQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju klijenata: {ex.Message}", Severity.Error);
            Console.WriteLine($"Error loading clients: {ex}");
        }
        finally
        {
            _isLoading = false;
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
            var matchesRisk = _riskFilter == "all" || c.RiskLevel == _riskFilter;

            return matchesSearch && matchesLegalForm && matchesRisk;
        });
    }

    private void ClearFilters()
    {
        _legalFormFilter = "all";
        _riskFilter = "all";
    }

    private int GetActiveFiltersCount()
    {
        int count = 0;
        if (_legalFormFilter != "all") count++;
        if (_riskFilter != "all") count++;
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
    private Color GetRiskColor(string risk) => risk switch
    {
        "NIZAK" => Color.Success,
        "SREDNJI" => Color.Warning,
        "VISOK" => Color.Error,
        _ => Color.Default
    };
}