using System.Text.Json;
using Lumium.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace LumiumPortal.Web.Services;

public class AuthService(HttpClient httpClient, IJSRuntime jsRuntime, AuthenticationStateProvider authStateProvider)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
    
    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        var response = await httpClient.PostAsJsonAsync("/api/auth/login", request);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<LoginResponse>();
    }

    public async Task SaveTokenAsync(string token)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt_token", token);
        ((CustomAuthenticationStateProvider)authStateProvider).NotifyUserAuthentication(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "jwt_token");
    }

    public async Task RemoveTokenAsync()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwt_token");
        ((CustomAuthenticationStateProvider)authStateProvider).NotifyUserLogout();
    }

    public async Task SaveLastCompanyAsync(string identifier, string name)
    {
        try
        {
            var company = new CompanyInfo(identifier, name);
            var json = JsonSerializer.Serialize(company, JsonOptions);
            
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", "last_company", json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SaveLastCompany error: {ex.Message}");
        }
    }

    public async Task<(string? identifier, string? name)> GetLastCompanyAsync()
    {
        var json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "last_company");

        if (string.IsNullOrEmpty(json))
        {
            return (null, null);
        }

        var company = JsonSerializer.Deserialize<CompanyInfo>(json, JsonOptions);
        return (company?.Identifier, company?.Name);
    }

    public async Task<ValidateCompanyResponse?> ValidateCompanyAsync(string companyName)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                "/api/auth/validate-company",
                new ValidateCompanyRequest(companyName));

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ValidateCompanyResponse>();
        }
        catch
        {
            return null;
        }
    }
}