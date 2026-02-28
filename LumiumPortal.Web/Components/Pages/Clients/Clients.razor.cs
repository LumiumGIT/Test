using Domain.Enums;
using Domain.Enums.Clients;
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
    
    private MudDataGrid<ClientDto>? _dataGrid;
    private List<ClientDto> _clients = [];
    private readonly HashSet<Guid> _selectedClients = [];
    private bool _isLoading = true;

    protected override async Task OnSecureInitializedAsync()
    {
        _isLoading = true;
        await LoadClients();
        _isLoading = false;
    }

    private async Task LoadClients()
    {
        _clients = await Mediator.Send(new GetClientsQuery());
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

    private bool IsAllSelected()
    {
        if (_dataGrid?.FilteredItems == null)
        {
            return false;
        }
    
        var filteredClients = _dataGrid.FilteredItems.ToList();
        
        return filteredClients.Count != 0 && filteredClients.All(c => _selectedClients.Contains(c.Id));
    }

    private void ToggleSelectAll()
    {
        if (_dataGrid?.FilteredItems == null)
        {
            return;
        }

        var filteredClients = _dataGrid.FilteredItems.ToList();
    
        if (IsAllSelected())
        {
            foreach (var client in filteredClients)
            {
                _selectedClients.Remove(client.Id);
            }
        }
        else
        {
            foreach (var client in filteredClients)
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

    private void ViewClient(Guid id)
    {
        Console.WriteLine($"View client: {id}");
    }

    private Color GetRiskColor(RiskLevel risk) => risk switch
    {
        RiskLevel.Low => Color.Success,
        RiskLevel.Medium => Color.Warning,
        RiskLevel.High => Color.Error,
        _ => Color.Default
    };
}