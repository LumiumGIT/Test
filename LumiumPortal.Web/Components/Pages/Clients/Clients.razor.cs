using Domain.Enums;
using Lumium.Application.Features.Clients.Commands;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using LumiumPortal.Web.Extensions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class Clients : SecureComponentBase 
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    
    private List<ClientDto> _clients = [];
    
    private string _searchQuery = "";
    
    private readonly HashSet<Guid> _selectedClients = [];

    protected override async Task OnSecureInitializedAsync()
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

            return matchesSearch;
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
    
    private async Task DeleteSelectedClients()
    {
        if (_selectedClients.Count == 0)
        {
            Snackbar.Add("Nema selektovanih klijenata", Severity.Warning);
            return;
        }

        var confirmed = await DialogService.ShowDeleteConfirmAsync("klijenta", _selectedClients.Count);

        if (!confirmed) return;

        try
        {
            var command = new DeleteClientsCommand(_selectedClients);
            var result = await Mediator.Send(command);

            if (result.IsSuccess)
            {
                Snackbar.Add(result.Message, Severity.Success);
            
                _selectedClients.Clear();
                await LoadClients();
            }
            else
            {
                Snackbar.Add(result.Message, Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška: {ex.Message}", Severity.Error);
        }
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