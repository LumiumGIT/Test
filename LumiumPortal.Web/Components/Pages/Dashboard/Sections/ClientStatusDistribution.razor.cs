using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class ClientStatusDistribution : ComponentBase
{
    [Parameter] public List<ClientStatusDistributionDto> StatusDistribution { get; set; } = [];
}