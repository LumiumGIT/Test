using LumiumPortal.Web.Components.Shared;
using MudBlazor;

namespace LumiumPortal.Web.Helpers;

public static class DialogHelper
{
    public static async Task<bool> ShowConfirmDialog(
        IDialogService dialogService,
        string message,
        string title = "Potvrda",
        string confirmText = "Potvrdi",
        Color confirmColor = Color.Primary)
    {
        var parameters = new DialogParameters
        {
            { nameof(ConfirmDialog.Message), message },
            { nameof(ConfirmDialog.ConfirmText), confirmText },
            { nameof(ConfirmDialog.ConfirmColor), confirmColor }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>(title, parameters, options);
        var result = await dialog.Result;

        return result is { Canceled: false };
    }
}