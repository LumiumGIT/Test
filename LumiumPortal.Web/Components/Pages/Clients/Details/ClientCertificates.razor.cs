using Lumium.Application.Features.Certificates.Commands;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Certificates.Queries;
using LumiumPortal.Web.Helpers.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientCertificates : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; } = null!;

    [Parameter] public Guid ClientId { get; set; }

    private List<CertificateDto> _certificates = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadCertificates();

        await base.OnInitializedAsync();
    }

    private async Task LoadCertificates()
    {
        try
        {
            _certificates = await Mediator.Send(new GetCertificatesByClientQuery(ClientId));
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Greška pri učitavanju sertifikata: {ex.Message}", Severity.Error);
            Console.WriteLine($"[ERROR] Load contracts failed: {ex}");
        }
    }

    private async Task OpenAddCertificateDialog()
    {
        if (await DialogService.ShowAddCertificateDialog(ClientId))
        {
            await LoadCertificates();
        }
    }

    private async Task OpenEditCertificateDialog(CertificateDto certificate)
    {
        if (await DialogService.ShowEditCertificateDialog(certificate))
        {
            await LoadCertificates();
        }
    }
    
    private async Task DeleteCertificate(CertificateDto certificate)
    {
        if (await DialogService.ShowDeleteCertificateConfirmation(certificate.CertificateName))
        {
            await HandleDeleteCertificate(certificate.Id);
        }
    }

    private async Task HandleDeleteCertificate(Guid id)
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
}