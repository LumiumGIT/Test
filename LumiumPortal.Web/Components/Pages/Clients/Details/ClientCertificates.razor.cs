using Domain.Enums.Certificates;
using Lumium.Application.Features.Certificates.DTOs;
using Lumium.Application.Features.Certificates.Queries;
using LumiumPortal.Web.Components.Pages.Certificates;
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
        _certificates = await Mediator.Send(new GetCertificatesByClientQuery(ClientId));
    }
    
    private async Task OpenAddCertificateDialog()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddCertificateDialog.ClientId), ClientId}
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

    private Color GetCertificateStatusColor(CertificateStatus status)
    {
        return status switch
        {
            CertificateStatus.Valid => Color.Success,
            CertificateStatus.ExpiringSoon => Color.Warning,
            CertificateStatus.Expired => Color.Error,
            _ => Color.Default
        };
    }
}