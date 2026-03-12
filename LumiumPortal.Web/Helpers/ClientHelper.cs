using Domain.Enums.Clients;
using MudBlazor;

namespace LumiumPortal.Web.Helpers;

public static class ClientHelper
{
    public static Color GetRiskColor(RiskLevel risk) => risk switch
    {
        RiskLevel.Low => Color.Success,
        RiskLevel.Medium => Color.Warning,
        RiskLevel.High => Color.Error,
        _ => Color.Default
    };
}