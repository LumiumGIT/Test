using Domain.Enums.Contracts;
using MudBlazor;

namespace LumiumPortal.Web.Helpers;

public static class ContractHelpers
{
    public static Color GetStatusColor(ContractStatus status) => status switch
    {
        ContractStatus.Active => Color.Success,
        ContractStatus.Pending => Color.Warning,
        ContractStatus.Completed => Color.Info,
        ContractStatus.Cancelled => Color.Error,
        _ => Color.Default
    };
    
    public static string GetStatusText(ContractStatus status) => status switch
    {
        ContractStatus.Active => "Aktivan",
        ContractStatus.Pending => "Na čekanju",
        ContractStatus.Completed => "Završen",
        ContractStatus.Cancelled => "Otkazan",
        _ => "Nepoznat"
    };

    public static string GetTypeText(ContractType type) => type switch
    {
        ContractType.Recurring => "Ponavljajući",
        ContractType.OneTime => "Jednokratni",
        _ => "Nepoznat"
    };
}