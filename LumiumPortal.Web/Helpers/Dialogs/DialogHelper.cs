using LumiumPortal.Web.Components.Shared;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.Dialogs;

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
    
    public static async Task<bool> ShowDeleteConfirmDialog(IDialogService dialogService, string message)
    {
        var parameters = new DialogParameters
        {
            { nameof(ConfirmDialog.Title), "Brisanje" },
            { nameof(ConfirmDialog.Message), message },
            { nameof(ConfirmDialog.WarningText), "Ova akcija ne može biti poništena!" },
            { nameof(ConfirmDialog.ConfirmText), "Obriši" },
            { nameof(ConfirmDialog.ConfirmColor), Color.Error },
            { nameof(ConfirmDialog.Icon), Icons.Material.Filled.DeleteForever },
            { nameof(ConfirmDialog.IconColor), Color.Error }
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Small,
            FullWidth = true
        };

        var dialog = await dialogService.ShowAsync<ConfirmDialog>("", parameters, options);
        var result = await dialog.Result;

        return result is { Canceled: false };
    }
}