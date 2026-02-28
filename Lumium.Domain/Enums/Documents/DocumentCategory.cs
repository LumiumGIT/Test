using System.ComponentModel;

namespace Domain.Enums.Documents;

public enum DocumentCategory
{
    [Description("Finansijski")]
    Financial = 0,
    
    [Description("Poreski")]
    Tax = 1,
    
    [Description("Pravni")]
    Legal = 2,
    
    [Description("Ostalo")]
    Other = 3
}