using System.ComponentModel;

namespace Domain.Enums.Clients;

public enum ClientSubStatus
{
    // ===== Active =====
    [Description("Standardan")]
    Standard = 0,
    
    [Description("Novoosnovan")]
    NewlyEstablished = 1,
    
    [Description("Preuzet")]
    Acquired = 2,
    
    // ===== WindingDown =====
    [Description("Dobrovoljno zatvaranje")]
    VoluntaryLiquidation = 10,
    
    [Description("Stečaj")]
    Bankruptcy = 11,
    
    [Description("Restrukturiranje")]
    Restructuring = 12,
    
    [Description("Spajanje/Preuzimanje")]
    Merger = 13,
    
    // ===== Inactive/Closed =====
    [Description("-")]
    None = 99
}