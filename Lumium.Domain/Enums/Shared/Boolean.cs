using System.ComponentModel;

namespace Domain.Enums.Shared;

public enum Boolean
{
    [Description("Ne")]
    False = 0,
    
    [Description("Da")]
    True = 1,
}