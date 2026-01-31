using LumiumPortal.Web.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Login;

public partial class Login : ComponentBase
{
    [Inject] private AuthService AuthService { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private MudForm? _form;
    private bool _isValid;
    private bool _isLoading;
    private string _companyName = string.Empty;
    private string? _errorMessage;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                var (_, lastCompanyName) = await AuthService.GetLastCompanyAsync();
                
                if (!string.IsNullOrEmpty(lastCompanyName))
                {
                    _companyName = lastCompanyName;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading last company: {ex.Message}");
            }
        }
    }

    private void ClearError()
    {
        _errorMessage = null;
    }

    private async Task ContinueToLogin()
    {
        if (string.IsNullOrWhiteSpace(_companyName))
        {
            _errorMessage = "Molimo unesite validno ime kompanije";
            return;
        }

        _isLoading = true;
        _errorMessage = null;

        try
        {
            var result = await AuthService.ValidateCompanyAsync(_companyName);

            if (result == null)
            {
                _errorMessage = "Kompanija nije pronadjena. Molimo proverite ime i pokusajte ponovo.";
                Console.WriteLine("Company validation failed");
                return;
            }

            Console.WriteLine($"Company validated: {result.Name} ({result.Identifier})");

            await AuthService.SaveLastCompanyAsync(result.Identifier, result.Name);

            Navigation.NavigateTo($"/login-user");
        }
        catch (Exception ex)
        {
            _errorMessage = "Dogodila se greska. Molimo pokusajte ponovo.";
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }
}