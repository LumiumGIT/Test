using Lumium.Application.Features.Clients.DTOs;
using LumiumPortal.Web.Components.Pages.Clients;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.Dialogs;

public static class ClientDialogHelper
{
    extension(IDialogService dialogService)
    {
        public async Task<bool> ShowAddClientDialog()
        {
            var parameters = new DialogParameters
            {
                { nameof(ClientDialog.IsEditMode), false }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<ClientDialog>(
                "Dodaj klijenta",
                parameters,
                options);
        
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowEditClientDialog(ClientDto client)
        {
            var parameters = new DialogParameters
            {
                { nameof(ClientDialog.ExistingClient), client },
                { nameof(ClientDialog.IsEditMode), true }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<ClientDialog>(
                "Izmeni klijenta",
                parameters,
                options);
        
            var result = await dialog.Result;
            return result is { Canceled: false };
        }
        
        public async Task<bool> ShowDeleteClientConfirmation(string clientName)
        {
            return await DialogHelper.ShowDeleteConfirmDialog(
                dialogService,
                message: $"Da li ste sigurni da želite da obrišete klijenta '{clientName}'?",
                requireTextMatch: "DA",
                matchPlaceholder: "Unesite 'DA' velikim slovima"
            );
        }
    }
}