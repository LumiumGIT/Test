using Domain.Enums;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Certificates.Queries;
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

    private string GetStatusColor(CertificateStatus status) => status switch
    {
        CertificateStatus.Expired => "var(--mud-palette-error)",           
        CertificateStatus.AboutToExpire => "var(--mud-palette-warning)",  
        CertificateStatus.ExpiringSoon => "var(--mud-palette-info)", 
        CertificateStatus.Valid => "var(--mud-palette-success)",           
        _ => "var(--mud-palette-text-secondary)"
    };

    private string GetStatusText(CertificateStatus status) => status switch
    {
        CertificateStatus.Expired => "Istekao",
        CertificateStatus.AboutToExpire => "Kritično (≤7 dana)",
        CertificateStatus.ExpiringSoon => "Upozorenje (≤30 dana)",
        CertificateStatus.Valid => "Validan",
        _ => string.Empty
    };

    private string GetDaysText(int days) => days < 0
        ? $"Istekao pre {Math.Abs(days)}d"
        : $"{days} dana";

    private string GetDaysChipStyle(CertificateStatus status) => status switch
    {
        CertificateStatus.Expired => "border-color: var(--mud-palette-error); color: var(--mud-palette-error);",
        CertificateStatus.AboutToExpire => "border-color: var(--mud-palette-warning); color: var(--mud-palette-warning);",
        CertificateStatus.ExpiringSoon => "border-color: var(--mud-palette-info); color: var(--mud-palette-info);",
        CertificateStatus.Valid => "border-color: var(--mud-palette-success); color: var(--mud-palette-success);",
        _ => string.Empty
    };

    private string GetRowStyle(CertificateDto cert, int index) => cert.Status switch
    {
        CertificateStatus.Expired => "background-color: var(--mud-palette-error-hover);",
        CertificateStatus.AboutToExpire => "background-color: var(--mud-palette-warning-hover);",
        CertificateStatus.ExpiringSoon => "background-color: var(--mud-palette-info-hover);",
        _ => string.Empty
    };
}