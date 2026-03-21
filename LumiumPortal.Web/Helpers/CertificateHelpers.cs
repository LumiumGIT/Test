using Domain.Enums.Certificates;

namespace LumiumPortal.Web.Helpers;

public static class CertificateHelpers
{
    public static string GetStatusColor(CertificateStatus status) =>
        status switch
        {
            CertificateStatus.Expired => "var(--mud-palette-error)",
            CertificateStatus.AboutToExpire => "var(--mud-palette-warning)",
            CertificateStatus.ExpiringSoon => "var(--mud-palette-info)",
            CertificateStatus.Valid => "var(--mud-palette-success)",
            _ => "var(--mud-palette-text-secondary)"
        };

    public static string GetStatusText(CertificateStatus status) =>
        status switch
        {
            CertificateStatus.Expired => "Istekao",
            CertificateStatus.AboutToExpire => "Kritično (≤7 dana)",
            CertificateStatus.ExpiringSoon => "Upozorenje (≤30 dana)",
            CertificateStatus.Valid => "Validan",
            _ => string.Empty
        };

    public static string GetDaysText(int days) =>
        days < 0
            ? $"Istekao pre {Math.Abs(days)} dana"
            : $"{days} dana";

    public static string GetDaysChipStyle(CertificateStatus status) =>
        status switch
        {
            CertificateStatus.Expired => "border-color: var(--mud-palette-error); color: var(--mud-palette-error);",
            CertificateStatus.AboutToExpire =>
                "border-color: var(--mud-palette-warning); color: var(--mud-palette-warning);",
            CertificateStatus.ExpiringSoon => "border-color: var(--mud-palette-info); color: var(--mud-palette-info);",
            CertificateStatus.Valid => "border-color: var(--mud-palette-success); color: var(--mud-palette-success);",
            _ => string.Empty
        };
}