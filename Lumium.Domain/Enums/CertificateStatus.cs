using System.ComponentModel;

namespace Domain.Enums;

public enum CertificateStatus
{
    [Description("Važeći")]
    Valid = 0,
    
    [Description("Ističe uskoro")]
    ExpiringSoon = 1,
    
    [Description("Istekao")]
    Expired = 2
}