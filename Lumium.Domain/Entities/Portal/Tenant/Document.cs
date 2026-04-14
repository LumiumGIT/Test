using Domain.Common;
using Domain.Enums.Documents;

namespace Domain.Entities.Portal.Tenant;

public class Document : TenantEntity
{
    public Guid ClientId { get; set; }

    public string Name { get; set; } = string.Empty;
    public DocumentCategory Category { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; }

    // Navigation
    public Client Client { get; set; } = null!;
}