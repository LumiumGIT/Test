using System.ComponentModel;

namespace Domain.Enums.Contracts;

public enum ContractType
{
    [Description("Ponavljajući")] Recurring = 0,

    [Description("Jednokratni")] OneTime = 1
}