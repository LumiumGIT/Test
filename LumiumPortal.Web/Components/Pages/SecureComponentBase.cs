using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace LumiumPortal.Web.Components.Pages;

public abstract class SecureComponentBase : ComponentBase
{
    [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
    [Inject] protected NavigationManager Navigation { get; set; } = null!;

    private bool IsAuthenticated { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        IsAuthenticated = authState.User.Identity?.IsAuthenticated ?? false;

        if (!IsAuthenticated)
        {
            Navigation.NavigateTo("/login", forceLoad: false);
            return;
        }

        await OnSecureInitializedAsync();
    }

    protected virtual Task OnSecureInitializedAsync()
    {
        return Task.CompletedTask;
    }
}