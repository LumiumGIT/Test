using System.ComponentModel;

namespace Domain.Enums;

public enum Boolean
{
    [Description("Ne")]
    False = 0,
    
    [Description("Da")]
    True = 1,
}