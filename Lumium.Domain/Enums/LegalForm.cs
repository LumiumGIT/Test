using System.ComponentModel;

namespace Domain.Enums;

public enum LegalForm
{
    [Description("Preduzetnik")] Entrepreneur = 0,

    [Description("Državni organ")] GovernmentBody = 1,

    [Description("Udruženje građana - Nevladina organizacija")]
    NonGovernmentalOrganization = 2
}