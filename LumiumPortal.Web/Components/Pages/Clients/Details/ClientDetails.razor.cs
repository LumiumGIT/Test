using Lumium.Application.Features.Clients.DTOs;
using Lumium.Application.Features.Clients.Queries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientDetails : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

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

        // Dummy data
        _clientDetails = await Mediator.Send(new GetClientDetailsQuery(ClientId));

        if (_clientDetails == null)
        {
            Snackbar.Add("Klijent nije pronađen", Severity.Error);
            return;
        }

        _clientDetails.Industry = "Informacione tehnologije";
        _clientDetails.CompanySize = "50-100 zaposlenih";
        _clientDetails.MonthlyFee = 150000;
        _clientDetails.BillingContact = "finance@techcorp.rs";
        _clientDetails.AssignedTo = "Sarah Mitchell";

        _isLoading = false;
    }

    private void HandleAddContract() => Snackbar.Add("Funkcionalnost 'Novi ugovor' - uskoro", Severity.Info);

    private void HandleAddCertificate() => Snackbar.Add("Funkcionalnost 'Dodaj sertifikat' - uskoro", Severity.Info);

    private void HandleUploadDocument() => Snackbar.Add("Funkcionalnost 'Otpremi dokument' - uskoro", Severity.Info);
}