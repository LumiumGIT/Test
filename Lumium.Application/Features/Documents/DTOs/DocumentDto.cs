using Domain.Enums.Documents;

namespace Lumium.Application.Features.Documents.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public DocumentCategory Category { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime UploadedAt { get; set; }
}