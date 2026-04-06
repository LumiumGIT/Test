using Domain.Common;
using Domain.Enums.Clients;

namespace Domain.Entities.Portal.Tenant;

public class ClientContact : TenantEntity
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public ContactType Type { get; set; } = ContactType.Secondary;

    public Guid? UpdatedBy { get; set; }

    // Navigation
    public Client Client { get; set; } = null!;
    public User? UpdatedByUser { get; set; }
}