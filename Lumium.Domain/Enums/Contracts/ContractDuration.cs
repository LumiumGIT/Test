using System.ComponentModel;

namespace Domain.Enums.Contracts;

public enum ContractDuration
{
    [Description("Vremenski određen")]
    Fixed = 0,
    
    [Description("Na neodređeno")]
    Indefinite = 1 
}