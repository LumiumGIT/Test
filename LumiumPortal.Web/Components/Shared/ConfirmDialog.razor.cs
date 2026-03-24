using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace LumiumPortal.Web.Components.Shared;

public partial class ConfirmDialog : ComponentBase
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

    [Parameter] public string Title { get; set; } = "Potvrdite akciju";
    [Parameter] public string Message { get; set; } = "Da li ste sigurni da želite da nastavite?";
    [Parameter] public string? WarningText { get; set; }
    [Parameter] public string ConfirmText { get; set; } = "Potvrdi";
    [Parameter] public string CancelText { get; set; } = "Otkaži";
    [Parameter] public Color ConfirmColor { get; set; } = Color.Primary;
    [Parameter] public string? Icon { get; set; }
    [Parameter] public Color IconColor { get; set; } = Color.Default;
    [Parameter] public string? RequireTextMatch { get; set; }
    [Parameter] public string? MatchPlaceholder { get; set; }

    private string _confirmInput = string.Empty;

    private bool IsConfirmInputValid =>
        string.IsNullOrEmpty(RequireTextMatch) ||
        _confirmInput.Trim().Equals(RequireTextMatch, StringComparison.Ordinal);

    private bool IsConfirmEnabled =>
        string.IsNullOrEmpty(RequireTextMatch) || IsConfirmInputValid;

    private string GetValidationIcon()
    {
        if (string.IsNullOrEmpty(_confirmInput)) 
            return Icons.Material.Outlined.Circle;
        
        return IsConfirmInputValid 
            ? Icons.Material.Filled.CheckCircle 
            : Icons.Material.Filled.Cancel;
    }

    private Color GetValidationColor()
    {
        if (string.IsNullOrEmpty(_confirmInput)) 
            return Color.Default;
        
        return IsConfirmInputValid 
            ? Color.Success 
            : Color.Error;
    }

    private string GetConfirmButtonStyle()
    {
        if (!IsConfirmEnabled)
            return "opacity: 0.5;";

        if (ConfirmColor == Color.Error)
            return "box-shadow: 0 4px 12px rgba(244, 67, 54, 0.3);";

        return "";
    }

    private void Confirm()
    {
        if (IsConfirmEnabled)
        {
            MudDialog.Close(DialogResult.Ok(true));
        }
    }

    private void Cancel() => MudDialog.Cancel();

    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" && IsConfirmEnabled)
        {
            Confirm();
        }
    }
}