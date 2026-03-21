using Domain.Enums.Shared;
using Lumium.Application.Features.Dashboard.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Dashboard.Sections;

public partial class UpcomingDeadlines : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public List<UpcomingDeadlineDto> Deadlines { get; set; } = [];

    private void OnDeadlineClick(TableRowClickEventArgs<UpcomingDeadlineDto> args)
    {
        var deadline = args.Item;

        var tabIndex = deadline?.Type == DeadlineType.Contract ? 1 : 2;

        NavigationManager.NavigateTo($"/clients/{deadline?.ClientId}?tab={tabIndex}");
    }

    private Color GetDeadlineColor(DeadlineStatus status) =>
        status switch
        {
            DeadlineStatus.Critical => Color.Error,
            DeadlineStatus.Warning => Color.Warning,
            DeadlineStatus.Info => Color.Info,
            _ => Color.Default
        };
}