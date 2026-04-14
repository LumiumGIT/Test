using Lumium.Application.Features.ClientContacts.DTOs;
using LumiumPortal.Web.Components.Pages.Clients.Details.ClientContacts;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.Dialogs;

public static class ClientContactDialogHelper
{
    extension(IDialogService dialogService)
    {
        public async Task<bool> ShowAddContactDialog(Guid clientId)
        {
            var parameters = new DialogParameters
            {
                { nameof(ClientContactDialog.ClientId), clientId },
                { nameof(ClientContactDialog.IsEditMode), false }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await dialogService.ShowAsync<ClientContactDialog>("Dodaj kontakt", parameters, options);
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowEditContactDialog(ClientContactDto contact)
        {
            var parameters = new DialogParameters
            {
                { nameof(ClientContactDialog.ExistingContact), contact },
                { nameof(ClientContactDialog.IsEditMode), true }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            var dialog = await dialogService.ShowAsync<ClientContactDialog>("Izmeni kontakt", parameters, options);
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowDeleteContactConfirmation(string contactName)
        {
            return await DialogHelper.ShowDeleteConfirmDialog(
                dialogService,
                $"Da li ste sigurni da želite da obrišete kontakt '{contactName}'?",
                requireTextMatch: "DA",
                matchPlaceholder: "Unesite 'DA' velikim slovima");
        }
    }
}