using Lumium.Application.Common.Interfaces;

namespace Lumium.Infrastructure.MultiTenancy;

public class TenantContext : ITenantContext
{
    private Guid? _tenantId;
    private string? _schemaName;
    
    public Guid TenantId => _tenantId ?? throw new InvalidOperationException("Tenant nije setovan");
    public string? SchemaName => _schemaName ?? throw new InvalidOperationException("Tenant nije setovan");
    public bool IsResolved => _tenantId.HasValue;

    public void SetTenant(Guid? tenantId, string schemaName)
    {
        _tenantId = tenantId;
        _schemaName = schemaName;
    }
}