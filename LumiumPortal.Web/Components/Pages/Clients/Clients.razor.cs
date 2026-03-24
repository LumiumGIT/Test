using Lumium.Application.Features.Clients.Commands;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients;

public partial class Clients : SecureComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    private MudDataGrid<ClientDto>? _dataGrid;
    private List<ClientDto> _clients = [];
    private bool _isLoading = true;

    protected override async Task OnSecureInitializedAsync()
    {
        _isLoading = true;
        await LoadClients();
        _isLoading = false;
    }

    private async Task LoadClients() => _clients = await Mediator.Send(new GetClientsQuery());

    private async Task AddClient()
    {
        if (await DialogService.ShowAddClientDialog())
        {
            await LoadClients();
        }
    }
    
    private async Task EditClient(ClientDto client)
    {
        if (await DialogService.ShowEditClientDialog(client))
        {
            await LoadClients();
        }
    }

    private async Task DeleteClient(ClientDto client)
    {
        if (!await DialogService.ShowDeleteClientConfirmation(client.Name))
        {
            return;
        }

        var result = await Mediator.Send(new DeleteClientCommand(client.Id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
            await LoadClients();
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }
}