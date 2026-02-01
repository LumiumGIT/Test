using Lumium.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace LumiumAdmin.Web.Services;

public class AdminAuthService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authStateProvider)
{
    public async Task<AdminLoginResponse?> LoginAsync(string email, string password)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                "/api/auth/admin/login", 
                new AdminLoginRequest(email, password));

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<AdminLoginResponse>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login error: {ex.Message}");
            return null;
        }
    }

    public async Task SaveTokenAsync(string token)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "admin_jwt_token", token);
        ((AdminAuthenticationStateProvider)authStateProvider).NotifyUserAuthentication(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "admin_jwt_token");
        }
        catch
        {
            return null;
        }
    }

    public async Task RemoveTokenAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "admin_jwt_token");
        ((AdminAuthenticationStateProvider)authStateProvider).NotifyUserLogout();
    }
}