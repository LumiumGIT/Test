using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Login;

public partial class RedirectToLogin : ComponentBase
{
    protected override void OnInitialized()
    {
        Navigation.NavigateTo("/login", forceLoad: true);
    }
}