namespace Domain.Common;

public abstract class TenantEntity
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}