using Domain.Enums.Certificates;
using Lumium.Application.Features.Certificates.Commands;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Certificates.Queries;
using LumiumPortal.Web.Helpers;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Certificates;

public partial class Certificates : SecureComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;
    
    private List<CertificateDto> _certificates = [];
    private bool _isLoading = true;

    private int ExpiredCount => _certificates.Count(c => c.Status == CertificateStatus.Expired);
    private int AboutToExpire => _certificates.Count(c => c.Status == CertificateStatus.AboutToExpire);
    private int ExpiringSoonCount => _certificates.Count(c => c.Status == CertificateStatus.ExpiringSoon);
    private int ValidCount => _certificates.Count(c => c.Status == CertificateStatus.Valid);

    protected override async Task OnSecureInitializedAsync()
    {
        _isLoading = true;
        await LoadCertificates();
        _isLoading = false;
    }

    private async Task LoadCertificates()
    {
        _certificates = await Mediator.Send(new GetCertificatesQuery());
    }
    
    private async Task OpenAddCertificateDialog()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<AddCertificateDialog>("Dodaj sertifikat", options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadCertificates();
        }
    }
    
    private async Task OpenDeleteDialog(CertificateDto certificate)
    {
        var confirmed = await DialogHelper.ShowConfirmDialog(
            DialogService,
            message: $"Da li ste sigurni da želite da obrišete sertifikat '{certificate.CertificateName}'?",
            title: "Potvrda brisanja",
            confirmText: "Obriši",
            confirmColor: Color.Error
        );

        if (confirmed)
        {
            await DeleteCertificate(certificate.Id);
        }
    }
    
    private async Task DeleteCertificate(Guid id)
    {
        var result = await Mediator.Send(new DeleteCertificateCommand(id));

        if (result.IsSuccess)
        {
            Snackbar.Add(result.Message, Severity.Success);
            await LoadCertificates();
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }

    private string GetRowStyle(CertificateDto cert, int index) => cert.Status switch
    {
        CertificateStatus.Expired => "background-color: var(--mud-palette-error-hover);",
        CertificateStatus.AboutToExpire => "background-color: var(--mud-palette-warning-hover);",
        CertificateStatus.ExpiringSoon => "background-color: var(--mud-palette-info-hover);",
        _ => string.Empty
    };
}