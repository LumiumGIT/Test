using Domain.Enums.Certificates;
using Lumium.Application.Features.Certificates.Commands;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Certificates.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
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

    private async Task LoadCertificates() => _certificates = await Mediator.Send(new GetCertificatesQuery());

    private async Task AddCertificate()
    {
        if (await DialogService.ShowAddCertificateDialog())
        {
            await LoadCertificates();
        }
    }
    
    private async Task EditCertificate(CertificateDto certificate)
    {
        if (await DialogService.ShowEditCertificateDialog(certificate))
        {
            await LoadCertificates();
        }
    }

    private async Task DeleteCertificate(CertificateDto certificate)
    {
        if (!await DialogService.ShowDeleteCertificateConfirmation(certificate.CertificateName))
        {
            return;
        }

        var result = await Mediator.Send(new DeleteCertificateCommand(certificate.Id));

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
}