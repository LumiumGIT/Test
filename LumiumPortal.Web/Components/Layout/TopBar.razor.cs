using LumiumPortal.Web.Services;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Layout;

public partial class TopBar : ComponentBase
{
    [Inject] private CustomAuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    
    [Parameter] public EventCallback OnDrawerToggle { get; set; }
    [Parameter] public EventCallback OnThemeToggle { get; set; }
    [Parameter] public bool IsDarkMode { get; set; } 

    private async Task ToggleDrawer() => await OnDrawerToggle.InvokeAsync();
    private async Task ToggleTheme() => await OnThemeToggle.InvokeAsync();

    private string GetUserFullName(System.Security.Claims.ClaimsPrincipal user)
    {
        var firstName = user.FindFirst("given_name")?.Value ?? user.FindFirst("FirstName")?.Value ?? "";
        var lastName = user.FindFirst("family_name")?.Value ?? user.FindFirst("LastName")?.Value ?? "";

        if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
        {
            return $"{firstName} {lastName}";
        }

        return user.FindFirst("email")?.Value ?? "Korisnik";
    }

    private async Task HandleLogout()
    {
        await AuthStateProvider.Logout();
        Navigation.NavigateTo("/login", forceLoad: true);
    }
}