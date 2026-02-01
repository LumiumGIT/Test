using LumiumAdmin.Web.Services;
using Microsoft.AspNetCore.Components;

namespace LumiumAdmin.Web.Components.Pages.Login;

public partial class AdminLogin : ComponentBase
{
    [Inject] private AdminAuthService AuthService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    
    private bool _isLoading;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string? _errorMessage;

    private async Task HandleLogin()
    {
        if (string.IsNullOrWhiteSpace(_email) || string.IsNullOrWhiteSpace(_password))
        {
            _errorMessage = "Email i lozinka su obavezni";
            return;
        }

        _isLoading = true;
        _errorMessage = null;

        try
        {
            Console.WriteLine($"Attempting admin login: {_email}");
            
            var response = await AuthService.LoginAsync(_email, _password);

            if (response == null)
            {
                _errorMessage = "Nevalidni pristupni podaci";
                Console.WriteLine("Login failed");
                return;
            }

            Console.WriteLine("Login successful!");
            await AuthService.SaveTokenAsync(response.Token);
            
            Navigation.NavigateTo("/dashboard");
        }
        catch (Exception ex)
        {
            _errorMessage = "Došlo je do greške. Pokušajte ponovo.";
            Console.WriteLine($"Login error: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
}