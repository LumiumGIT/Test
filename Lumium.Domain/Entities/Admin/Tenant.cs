namespace Domain.Entities.Admin;

public class Tenant
{
    public Guid Id { get; set; }
    public string Identifier { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string SchemaName { get; set; } = null!; 
    public string ConnectionString { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}