using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class ClientSubStatusDistribution : ComponentBase
{
    [Parameter] public List<ClientSubStatusDistributionDto> SubStatusDistribution { get; set; } = [];
}