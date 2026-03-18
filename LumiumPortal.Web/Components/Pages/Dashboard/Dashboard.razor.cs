using Lumium.Application.Features.Dashboard.DTOs;
using Lumium.Application.Features.Dashboard.Queries;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Dashboard;

public partial class Dashboard : SecureComponentBase
{
    private DashboardDataDto _data = new();
    private bool _isLoading = true;

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
    
    private bool HasAlerts =>
        _data.Alerts.ExpiredCertificates.Count != 0 ||
        _data.Alerts.ExpiredContracts.Count != 0 ||
        _data.Alerts.ClientsWithoutDocuments.Count != 0;
    
    private bool HasRecentClients => _data.RecentClients.Count != 0;
    
    private bool HasAcquisitions => _data.AcquisitionsThisYear.Count != 0;
    
    private bool HasStatusDistribution => _data.ClientsByStatus.Count != 0;
    
    private bool HasSubStatusDistribution => _data.ClientsBySubStatus.Count != 0;
}