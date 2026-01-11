using Lumium.MultiTenancy.Interfaces;

namespace Lumium.MultiTenancy.Services;

public class TenantContext : ITenantContext
{
    public string? TenantId { get; private set; }
    public string? SchemaName { get; private set; }
    public bool IsResolved => !string.IsNullOrEmpty(TenantId);

    public void SetTenant(string tenantId, string schemaName)
    {
        TenantId = tenantId;
        SchemaName = schemaName;
    }
}