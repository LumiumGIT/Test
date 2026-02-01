using LumiumPortal.Web.Services;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages;

public partial class Dashboard : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private async Task Logout()
    {
        await AuthService.RemoveTokenAsync();
        Navigation.NavigateTo("/login", forceLoad: true);
    }
}