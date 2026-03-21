using Lumium.Application.Common.Models;
using Lumium.Application.Features.Certificates.Commands;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Certificates.Queries;
using LumiumPortal.Web.Components.Pages.Certificates;
using LumiumPortal.Web.Helpers;
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
        var parameters = new DialogParameters
        {
            { nameof(AddCertificateDialog.ClientId), ClientId }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = true,
            CloseOnEscapeKey = true
        };

        var dialog = await DialogService.ShowAsync<AddCertificateDialog>("Dodaj sertifikat", parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false })
        {
            await LoadCertificates();
        }
    }

    private async Task DeleteCertificate(CertificateDto certificate)
    {
        var confirmed = await DialogHelpers.ShowConfirmDialog(
            DialogService,
            $"Da li ste sigurni da želite da obrišete sertifikat '{certificate.CertificateName}'?",
            "Potvrda brisanja",
            "Obriši",
            Color.Error
        );

        if (confirmed)
        {
            var result = await Mediator.Send(new DeleteCertificateCommand(certificate.Id));

            await HandleResult(result);
        }
    }

    private async Task HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            await LoadCertificates();
            Snackbar.Add(result.Message, Severity.Success);
        }
        else
        {
            Snackbar.Add(result.Message, Severity.Error);
        }
    }
}