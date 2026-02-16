using System.ComponentModel;

namespace Domain.Enums;

public enum ContractType
{
    [Description("Ponavljajući")]
    Recurring = 0,
    
    [Description("Jednokratni")]
    OneTime = 1
}