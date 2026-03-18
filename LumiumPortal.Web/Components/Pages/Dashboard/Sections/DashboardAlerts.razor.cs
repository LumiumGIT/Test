using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class DashboardAlerts : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    [Parameter] public DashboardAlertsDto Alerts { get; set; } = new();

    private bool _showExpiredCertificates;
    private bool _showExpiredContracts;
    private bool _showClientsWithoutDocs;

    private void NavigateToClientTab(Guid clientId, int tabIndex)
    {
        NavigationManager.NavigateTo($"/clients/{clientId}?tab={tabIndex}");
    }
}