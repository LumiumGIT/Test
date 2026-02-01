using Microsoft.AspNetCore.Components;

namespace LumiumAdmin.Web.Components.Pages;

public partial class ProtectedPage : ComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}