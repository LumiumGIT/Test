using System.ComponentModel;

namespace Domain.Enums.Clients;

public enum RiskLevel
{
    [Description("Nizak")]
    Low = 0,
    
    [Description("Srednji")]
    Medium = 1,
    
    [Description("Visok")]
    High = 2
}