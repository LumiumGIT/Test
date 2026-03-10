using Lumium.Application.Features.Documents.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientDocuments : ComponentBase
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    
    [Parameter, EditorRequired] public List<DocumentDto> Documents { get; set; } = [];

    [Parameter] public EventCallback OnUploadDocument { get; set; }
    
    private async Task OpenDocument(string url)
    {
        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }
}