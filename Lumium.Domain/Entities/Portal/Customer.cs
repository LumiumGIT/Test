using Domain.Common;

namespace Domain.Entities.Portal;

public class Customer : TenantEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}