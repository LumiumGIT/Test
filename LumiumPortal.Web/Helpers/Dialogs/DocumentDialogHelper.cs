using Lumium.Application.Features.Documents.DTOs;
using LumiumPortal.Web.Components.Pages.Documents;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.Dialogs;

public static class DocumentDialogHelper
{
    extension(IDialogService dialogService)
    {
        public async Task<bool> ShowAddDocumentDialog(Guid clientId = default)
        {
            var parameters = new DialogParameters
            {
                { nameof(DocumentDialog.ClientId), clientId },
                { nameof(DocumentDialog.IsEditMode), false }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<DocumentDialog>(
                "Dodaj dokument",
                parameters,
                options);
        
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowEditDocumentDialog(DocumentDto document)
        {
            var parameters = new DialogParameters
            {
                { nameof(DocumentDialog.ExistingDocument), document },
                { nameof(DocumentDialog.IsEditMode), true }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<DocumentDialog>(
                "Izmeni dokument",
                parameters,
                options);
        
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowDeleteDocumentConfirmation(string documentName)
        {
            return await DialogHelper.ShowDeleteConfirmDialog(
                dialogService,
                message: $"Da li ste sigurni da želite da obrišete dokument '{documentName}'?"
            );
        }
    }
}