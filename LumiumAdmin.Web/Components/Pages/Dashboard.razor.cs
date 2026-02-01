using LumiumAdmin.Web.Services;
using Microsoft.AspNetCore.Components;

namespace LumiumAdmin.Web.Components.Pages;

public partial class Dashboard : ComponentBase
{
    [Inject] private AdminAuthService AuthService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var token = await AuthService.GetTokenAsync();
        
        if (string.IsNullOrEmpty(token))
        {
            Navigation.NavigateTo("/login");
        }
    }

    private async Task Logout()
    {
        await AuthService.RemoveTokenAsync();
        Navigation.NavigateTo("/login");
    }
}