using AutoMapper;
using Lumium.Application.Features.Clients.Commands;
using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientDetails : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private IMapper Mapper { get; set; } = null!;

    [Parameter] public Guid ClientId { get; set; }

    private ClientDetailsDto? _clientDetails;
    private bool _isLoading = true;
    private int _activeTabIndex = 0;
    private decimal TotalContractValue => _clientDetails?.Contracts.Sum(c => c.MonthlyFee) ?? 0;

    protected override async Task OnInitializedAsync()
    {
        var uri = new Uri(NavigationManager.Uri);
        var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);

        if (int.TryParse(queryParams["tab"], out var tabIndex))
        {
            _activeTabIndex = tabIndex;
        }

        await LoadClientDetails();
    }

    private async Task LoadClientDetails()
    {
        _isLoading = true;

        _clientDetails = await Mediator.Send(new GetClientDetailsQuery(ClientId));

        if (_clientDetails == null)
        {
            Snackbar.Add("Klijent nije pronađen", Severity.Error);
            return;
        }

        _isLoading = false;
    }
    
    private async Task EditClient()
    {
        if (_clientDetails == null)
        {
            return;
        }

        var clientDto = Mapper.Map<ClientDto>(_clientDetails);

        if (await DialogService.ShowEditClientDialog(clientDto))
        {
            await LoadClientDetails();
        }
    }
    
    private async Task DeleteClient()
    {
        if (_clientDetails == null)
        {
            return;
        }
        
        if (!await DialogService.ShowDeleteClientConfirmation(_clientDetails.Name))
        {
            return;
        }

        var result = await Mediator.Send(new DeleteClientCommand(_clientDetails.Id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
            Navigation.NavigateTo("/clients");
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }
}