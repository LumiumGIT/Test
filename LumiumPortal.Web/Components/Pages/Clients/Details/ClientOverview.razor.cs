using Lumium.Application.Features.Clients.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientOverview : ComponentBase
{
    [Inject] private IJSRuntime Js { get; set; } = null!;
    
    [Parameter] [EditorRequired] public ClientDetailsDto Client { get; set; } = null!;
    
    private async Task OpenInMaps()
    {
        var query = Uri.EscapeDataString($"{Client.Address}, {Client.Country?.Name}");
        var url = $"https://www.google.com/maps/search/?api=1&query={query}";
        await Js.InvokeVoidAsync("open", url, "_blank");
    }
}