using System.ComponentModel;

namespace Domain.Enums.Clients;

public enum ContactType
{
    [Description("Glavni")] Primary = 1,
        
    [Description("Sporedni")] Secondary = 2
}