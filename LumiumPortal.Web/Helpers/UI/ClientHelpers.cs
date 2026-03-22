using Domain.Enums.Clients;
using Lumium.Application.Common.Extensions;
using MudBlazor;

namespace LumiumPortal.Web.Helpers.UI;

public static class ClientHelpers
{
    public static Color GetRiskColor(RiskLevel risk) =>
        risk switch
        {
            RiskLevel.Low => Color.Success,
            RiskLevel.Medium => Color.Warning,
            RiskLevel.High => Color.Error,
            _ => Color.Default
        };

    public static Color GetStatusColor(ClientStatus status) =>
        status switch
        {
            ClientStatus.Active => Color.Success,
            ClientStatus.Inactive => Color.Default,
            ClientStatus.WindingDown => Color.Warning,
            ClientStatus.Closed => Color.Error,
            _ => Color.Default
        };

    public static string GetStatusIcon(ClientStatus status) =>
        status switch
        {
            ClientStatus.Active => Icons.Material.Filled.CheckCircle,
            ClientStatus.Inactive => Icons.Material.Filled.PauseCircle,
            ClientStatus.WindingDown => Icons.Material.Filled.Warning,
            ClientStatus.Closed => Icons.Material.Filled.Cancel,
            _ => Icons.Material.Filled.Help
        };

    public static Color GetSubStatusColor(ClientSubStatus subStatus) =>
        subStatus switch
        {
            ClientSubStatus.Standard => Color.Default,
            ClientSubStatus.NewlyEstablished => Color.Info,
            ClientSubStatus.Acquired => Color.Primary,
            ClientSubStatus.VoluntaryLiquidation => Color.Warning,
            ClientSubStatus.Bankruptcy => Color.Error,
            ClientSubStatus.Restructuring => Color.Warning,
            ClientSubStatus.Merger => Color.Info,
            _ => Color.Default
        };

    public static bool IsValidSubStatusForStatus(ClientStatus status, ClientSubStatus subStatus) =>
        (status, subStatus) switch
        {
            // Active može da ima Standard, NewlyEstablished, Acquired
            (ClientStatus.Active, ClientSubStatus.Standard) => true,
            (ClientStatus.Active, ClientSubStatus.NewlyEstablished) => true,
            (ClientStatus.Active, ClientSubStatus.Acquired) => true,

            // WindingDown može da ima liquidation, bankruptcy, restructuring, merger
            (ClientStatus.WindingDown, ClientSubStatus.VoluntaryLiquidation) => true,
            (ClientStatus.WindingDown, ClientSubStatus.Bankruptcy) => true,
            (ClientStatus.WindingDown, ClientSubStatus.Restructuring) => true,
            (ClientStatus.WindingDown, ClientSubStatus.Merger) => true,

            // Inactive i Closed imaju samo None
            (ClientStatus.Inactive, ClientSubStatus.None) => true,
            (ClientStatus.Closed, ClientSubStatus.None) => true,

            _ => false
        };

    public static List<ClientSubStatus> GetValidSubStatusesForStatus(ClientStatus status) =>
        status switch
        {
            ClientStatus.Active =>
            [
                ClientSubStatus.Standard,
                ClientSubStatus.NewlyEstablished,
                ClientSubStatus.Acquired
            ],
            ClientStatus.WindingDown =>
            [
                ClientSubStatus.VoluntaryLiquidation,
                ClientSubStatus.Bankruptcy,
                ClientSubStatus.Restructuring,
                ClientSubStatus.Merger
            ],
            ClientStatus.Inactive or ClientStatus.Closed => [ClientSubStatus.None],
            _ => []
        };

    public static string GetFullStatusText(ClientStatus status, ClientSubStatus subStatus)
    {
        var statusText = status.GetDescription();
        var subStatusText = subStatus.GetDescription();

        return subStatus is ClientSubStatus.None or ClientSubStatus.Standard
            ? statusText
            : $"{statusText} ({subStatusText})";
    }
}