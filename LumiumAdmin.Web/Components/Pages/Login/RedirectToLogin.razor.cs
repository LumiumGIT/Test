using Microsoft.AspNetCore.Components;

namespace LumiumAdmin.Web.Components.Pages.Login;

public partial class RedirectToLogin : ComponentBase
{
    protected override void OnInitialized()
    {
        Navigation.NavigateTo("/login", forceLoad: true);
    }
}