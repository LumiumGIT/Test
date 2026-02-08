using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Shared;

public partial class ConfirmDialog : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public string Title { get; set; } = "Potvrdite akciju";

    [Parameter]
    public string Message { get; set; } = "Da li ste sigurni da želite da nastavite?";

    [Parameter]
    public string? WarningText { get; set; }

    [Parameter]
    public string ConfirmText { get; set; } = "Potvrdi";

    [Parameter]
    public string CancelText { get; set; } = "Otkaži";

    [Parameter]
    public Color ConfirmColor { get; set; } = Color.Primary;

    [Parameter]
    public string? Icon { get; set; }

    [Parameter]
    public Color IconColor { get; set; } = Color.Default;

    private void Confirm() => MudDialog.Close(DialogResult.Ok(true));
    private void Cancel() => MudDialog.Cancel();
}