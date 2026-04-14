using Lumium.Application.Features.Contracts.DTOs;
using LumiumPortal.Web.Components.Pages.Contracts;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.Dialogs;

public static class ContractDialogHelper
{
    extension(IDialogService dialogService)
    {
        public async Task<bool> ShowAddContractDialog(Guid clientId = default)
        {
            var parameters = new DialogParameters
            {
                { nameof(ContractDialog.ClientId), clientId },
                { nameof(ContractDialog.IsEditMode), false }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<ContractDialog>(
                "Dodaj ugovor",
                parameters,
                options);

            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowAddAnnexDialog(ContractDto parentContract)
        {
            var parameters = new DialogParameters
            {
                { nameof(ContractDialog.ClientId), parentContract.ClientId },
                { nameof(ContractDialog.ParentContractId), parentContract.Id },
                { nameof(ContractDialog.ParentContractNumber), parentContract.ContractNumber },
                { nameof(ContractDialog.IsEditMode), false }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<ContractDialog>(
                $"Dodaj aneks — {parentContract.ContractNumber}",
                parameters,
                options);

            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowEditContractDialog(ContractDto contract)
        {
            var parameters = new DialogParameters
            {
                { nameof(ContractDialog.ExistingContract), contract },
                { nameof(ContractDialog.IsEditMode), true }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseButton = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<ContractDialog>(
                "Izmeni ugovor",
                parameters,
                options);

            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowDeleteContractConfirmation(string contractNumber)
        {
            return await DialogHelper.ShowDeleteConfirmDialog(
                dialogService,
                message: $"Da li ste sigurni da želite da obrišete ugovor '{contractNumber}'?",
                requireTextMatch: "DA",
                matchPlaceholder: "Unesite 'DA' velikim slovima"
            );
        }
    }
}