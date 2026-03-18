using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class ClientAcquisitions : ComponentBase
{
    [Parameter] public List<ClientAcquisitionDto> Acquisitions { get; set; } = [];
}