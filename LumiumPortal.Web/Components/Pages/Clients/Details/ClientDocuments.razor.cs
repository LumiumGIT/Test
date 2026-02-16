using Lumium.Application.Features.Clients.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientDocuments : ComponentBase
{
    [Parameter, EditorRequired] public List<DocumentDto> Documents { get; set; } = [];

    [Parameter]
    public EventCallback OnUploadDocument { get; set; }

    [Parameter]
    public EventCallback<Guid> OnDownloadDocument { get; set; }
}