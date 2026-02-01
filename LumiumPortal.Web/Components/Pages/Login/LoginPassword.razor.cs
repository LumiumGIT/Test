using Lumium.Contracts;
using LumiumPortal.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Login;

public partial class LoginPassword : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = null!;

    private MudForm? _form;
    private bool _isValid;
    private bool _isLoading;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string? _errorMessage;
    private string _tenantName = string.Empty;
    private string _tenantIdentifier = string.Empty;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var (identifier, name) = await AuthService.GetLastCompanyAsync();

            _tenantName = name ?? string.Empty;
            _tenantIdentifier = identifier ?? string.Empty;

            StateHasChanged();
        }
    }

    private string? ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return "Email je obavezan";
        }

        return !email.Contains('@') ? "Pogresan format email-a" : null;
    }

    private void GoBack()
    {
        Navigation.NavigateTo("/login");
    }

    private async Task HandleLogin()
    {
        if (!_isValid)
        {
            return;
        }

        _isLoading = true;
        _errorMessage = null;

        try
        {
            var request = new LoginRequest(_tenantIdentifier, _email, _password);
            var response = await AuthService.LoginAsync(request);

            if (response == null)
            {
                _errorMessage = "Pogresan email ili lozinka";
                return;
            }

            // Save JWT token
            await AuthService.SaveTokenAsync(response.Token);

            // Navigate to dashboard
            Navigation.NavigateTo("/dashboard");
        }
        catch (Exception ex)
        {
            _errorMessage = "Dogodila se greska. Molimo pokusajte ponovo.";
            Console.WriteLine($"Login error: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
}