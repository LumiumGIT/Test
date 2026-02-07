namespace Lumium.Application.Common.Interfaces;

public interface ITenantContext
{
    Guid TenantId { get; }
    string? SchemaName { get; }
    bool IsResolved { get; }
}