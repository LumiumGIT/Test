namespace Lumium.Application.Features.Dashboard.DTOs;

public class DashboardAlertsDto
{
    public List<ExpiredCertificateAlertDto> ExpiredCertificates { get; set; } = [];
    public List<ExpiredContractAlertDto> ExpiredContracts { get; set; } = [];
    public List<ClientWithoutDocumentsAlertDto> ClientsWithoutDocuments { get; set; } = [];
}