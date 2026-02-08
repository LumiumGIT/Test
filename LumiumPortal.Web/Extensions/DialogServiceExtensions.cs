using LumiumPortal.Web.Components.Shared;
using MudBlazor;

namespace LumiumPortal.Web.Extensions;

public static class DialogServiceExtensions
{
     extension(IDialogService dialogService)
     {
         public async Task<bool> ShowConfirmAsync(string title,
             string message,
             string? warningText = null,
             string confirmText = "Potvrdi",
             string cancelText = "Otkaži",
             Color confirmColor = Color.Primary)
         {
             var parameters = new DialogParameters
             {
                 { nameof(ConfirmDialog.Title), title },
                 { nameof(ConfirmDialog.Message), message },
                 { nameof(ConfirmDialog.WarningText), warningText },
                 { nameof(ConfirmDialog.ConfirmText), confirmText },
                 { nameof(ConfirmDialog.CancelText), cancelText },
                 { nameof(ConfirmDialog.ConfirmColor), confirmColor }
             };

             var options = new DialogOptions 
             { 
                 MaxWidth = MaxWidth.Small, 
                 FullWidth = true,
                 CloseButton = false 
             };

             var dialog = await dialogService.ShowAsync<ConfirmDialog>("", parameters, options);
             var result = await dialog.Result;

             return result is { Canceled: false };
         }

         public async Task<bool> ShowDeleteConfirmAsync(string itemName, int count = 1)
         {
             var parameters = new DialogParameters
             {
                 { nameof(ConfirmDialog.Title), "Brisanje" },
                 { nameof(ConfirmDialog.Message), count == 1 
                     ? $"Da li ste sigurni da želite da obrišete {itemName}?"
                     : $"Da li ste sigurni da želite da obrišete {count} stavki?" },
                 { nameof(ConfirmDialog.WarningText), "Ova akcija ne može biti poništena!" },
                 { nameof(ConfirmDialog.ConfirmText), "Obriši" },
                 { nameof(ConfirmDialog.CancelText), "Otkaži" },
                 { nameof(ConfirmDialog.ConfirmColor), Color.Error },
                 { nameof(ConfirmDialog.Icon), Icons.Material.Filled.DeleteForever },
                 { nameof(ConfirmDialog.IconColor), Color.Error }
             };

             var options = new DialogOptions 
             { 
                 MaxWidth = MaxWidth.Small, 
                 FullWidth = true,
                 CloseButton = false 
             };

             var dialog = await dialogService.ShowAsync<ConfirmDialog>("", parameters, options);
             var result = await dialog.Result;

             return result is { Canceled: false };
         }
     }
}