using Lumium.Application.Features.Certificates.DTOs;
using LumiumPortal.Web.Components.Pages.Certificates;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.Dialogs;

public static class CertificateDialogHelper
{
    extension(IDialogService dialogService)
    {
        public async Task<bool> ShowAddCertificateDialog(Guid clientId = default)
        {
            var parameters = new DialogParameters
            {
                { nameof(CertificateDialog.ClientId), clientId },
                { nameof(CertificateDialog.IsEditMode), false }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<CertificateDialog>(
                "Dodaj novi sertifikat",
                parameters,
                options);
        
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowEditCertificateDialog(CertificateDto certificate)
        {
            var parameters = new DialogParameters
            {
                { nameof(CertificateDialog.ExistingCertificate), certificate },
                { nameof(CertificateDialog.IsEditMode), true }
            };

            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                CloseOnEscapeKey = true
            };

            var dialog = await dialogService.ShowAsync<CertificateDialog>(
                "Izmeni sertifikat",
                parameters,
                options);
        
            var result = await dialog.Result;
            return result is { Canceled: false };
        }

        public async Task<bool> ShowDeleteCertificateConfirmation(string certificateName)
        {
            return await DialogHelper.ShowConfirmDialog(
                dialogService,
                message: $"Da li ste sigurni da želite da obrišete sertifikat '{certificateName}'?",
                title: "Potvrda brisanja",
                confirmText: "Obriši",
                confirmColor: Color.Error
            );
        }
    }
}