using System.ComponentModel;

namespace Domain.Enums.Clients;

public enum ClientStatus
{
    [Description("Aktivan")]
    Active = 0,
    
    [Description("Neaktivan")]
    Inactive = 1,
    
    [Description("U procesu gašenja")]
    WindingDown = 2,
    
    [Description("Ugašen")]
    Closed = 3
}