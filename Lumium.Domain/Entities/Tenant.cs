namespace Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Identifier { get; set; } = null!; // unique: tenant_abc
    public string Name { get; set; } = null!;
    public string SchemaName { get; set; } = null!; // tenant_abc_schema
    public string ConnectionString { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}