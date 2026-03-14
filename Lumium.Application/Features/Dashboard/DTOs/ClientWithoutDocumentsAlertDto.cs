namespace Lumium.Application.Features.Dashboard.DTOs;

public class ClientWithoutDocumentsAlertDto
{
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int CertificatesCount { get; set; }
    public int ContractsCount { get; set; }
}