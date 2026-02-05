// LumiumPortal.Web/Services/CustomAuthenticationStateProvider.cs

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Lumium.Application.Common.Interfaces;
using Lumium.Infrastructure.MultiTenancy;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace LumiumPortal.Web.Services;

public class CustomAuthenticationStateProvider(
    IJSRuntime jsRuntime,
    ITenantContext tenantContext) : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", "jwt_token");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(_anonymous);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwt_token");
                return new AuthenticationState(_anonymous);
            }

            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            ResolveTenantFromClaims(jwtToken.Claims);

            return new AuthenticationState(user);
        }
        catch (JSException)
        {
            return new AuthenticationState(_anonymous);
        }
        catch (InvalidOperationException)
        {
            return new AuthenticationState(_anonymous);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Auth error: {ex.Message}");
            return new AuthenticationState(_anonymous);
        }
    }

    public async Task Logout()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwt_token");
        NotifyUserLogout();
    }

    public void NotifyUserAuthentication(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);

        ResolveTenantFromClaims(jwtToken.Claims);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public void NotifyUserLogout()
    {
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }

    private void ResolveTenantFromClaims(IEnumerable<Claim> claims)
    {
        var tenantId = claims.FirstOrDefault(c => c.Type == "tenant_id")?.Value;
        var schemaName = claims.FirstOrDefault(c => c.Type == "schema_name")?.Value;

        if (!string.IsNullOrEmpty(tenantId) && !string.IsNullOrEmpty(schemaName))
        {
            ((TenantContext)tenantContext).SetTenant(tenantId, schemaName);
            Console.WriteLine($"[DEBUG] Tenant resolved: {tenantId}, schema: {schemaName}");
        }
        else
        {
            Console.WriteLine($"[DEBUG] Tenant claims missing - tenant_id: {tenantId}, schema_name: {schemaName}");
        }
    }
}