using Domain.Enums.Shared;
using Lumium.Application.Features.Dashboard.DTOs;
using Lumium.Application.Features.Dashboard.Queries;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Dashboard;

public partial class Dashboard : SecureComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    private DashboardDataDto _data = new();
    private bool _isLoading = true;
    
    private bool _showExpiredCertificates;
    private bool _showExpiredContracts;
    private bool _showClientsWithoutDocs;

    private string UserName => "Korisnik"; // TODO: Uzmi iz auth context-a
    
    private void ToggleExpiredCertificates() => _showExpiredCertificates = !_showExpiredCertificates;
    private void ToggleExpiredContracts() => _showExpiredContracts = !_showExpiredContracts;
    private void ToggleClientsWithoutDocs() => _showClientsWithoutDocs = !_showClientsWithoutDocs;

    private bool HasAlerts =>
        _data.Alerts.ExpiredCertificates.Count != 0 ||
        _data.Alerts.ExpiredContracts.Count != 0 ||
        _data.Alerts.ClientsWithoutDocuments.Count != 0;

    protected override async Task OnSecureInitializedAsync()
    {
        _isLoading = true;
        await LoadDashboardData();
        _isLoading = false;
    }

    private async Task LoadDashboardData()
    {
        try
        {
            _data = await Mediator.Send(new GetDashboardDataQuery());
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju dashboard-a: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load dashboard failed: {ex}");
        }
    }
    
    private void OnDeadlineClick(TableRowClickEventArgs<UpcomingDeadlineDto> args)
    {
        var deadline = args.Item;
        
        var tabIndex = deadline?.Type == DeadlineType.Certificate ? 2 : 1;
        
        NavigationManager.NavigateTo($"/clients/{deadline?.ClientId}?tab={tabIndex}");
    }
    
    private void NavigateToClientTab(Guid clientId, int tabIndex)
    {
        NavigationManager.NavigateTo($"/clients/{clientId}?tab={tabIndex}");
    }

    private Color GetDeadlineColor(DeadlineStatus status) => status switch
    {
        DeadlineStatus.Critical => Color.Error,
        DeadlineStatus.Warning => Color.Warning,
        DeadlineStatus.Info => Color.Info,
        _ => Color.Default
    };
}