using Microsoft.AspNetCore.Components;

namespace LumiumPortal.Web.Components.Shared;

public partial class LoadingIndicator : ComponentBase
{
    [Parameter] public string Text { get; set; } = "Učitavam...";
    [Parameter] public MudBlazor.Size Size { get; set; } = MudBlazor.Size.Large;
}