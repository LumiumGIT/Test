using System.ComponentModel;

namespace Domain.Enums.Contracts;

public enum ContractStatus
{
    [Description("Aktivan")]
    Active = 0,
    
    [Description("Završen")]
    Completed = 1,
    
    [Description("Na čekanju")]
    Pending = 2,
    
    [Description("Otkazan")]
    Cancelled = 3
}