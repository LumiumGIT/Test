using System.ComponentModel;

namespace Domain.Enums.Certificates;

public enum CertificateStatus
{
    [Description("Važeći")]
    Valid = 0,
    
    [Description("Ističe uskoro")]
    ExpiringSoon = 1,
    
    [Description("Samo što nije istekao")]
    AboutToExpire = 2,
    
    [Description("Istekao")]
    Expired = 3
}