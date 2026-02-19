using Domain.Enums;
using Lumium.Application.Features.Certificates.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LumiumPortal.Web.Components.Pages.Clients.Details;

public partial class ClientCertificates : ComponentBase
{
    [Parameter, EditorRequired] public List<CertificateDto> Certificates { get; set; } = [];

    [Parameter]
    public EventCallback OnAddCertificate { get; set; }

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