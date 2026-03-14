using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class DashboardStats : ComponentBase
{
    [Parameter] public DashboardStatsDto Stats { get; set; } = new();
}