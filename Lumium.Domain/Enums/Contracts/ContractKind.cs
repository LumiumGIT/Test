using System.ComponentModel;

namespace Domain.Enums.Contracts;

public enum ContractKind
{
    [Description("Osnovni")]
    Main = 1,
    [Description("Aneks")]
    Annex = 2
}