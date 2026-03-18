using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class RecentClients : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    
    [Parameter] public List<RecentClientDto> Clients { get; set; } = [];

    private void NavigateToClient(Guid clientId)
    {
        NavigationManager.NavigateTo($"/clients/{clientId}");
    }
}