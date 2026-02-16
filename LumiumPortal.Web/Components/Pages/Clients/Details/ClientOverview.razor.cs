using Lumium.Application.Features.Clients.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientOverview : ComponentBase
{
    [Parameter, EditorRequired] public ClientDetailsDto Client { get; set; } = null!;
}