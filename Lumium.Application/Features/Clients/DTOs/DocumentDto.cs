using Domain.Enums;

namespace Lumium.Application.Features.Clients.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DocumentCategory Category { get; set; }
    public DateTime UploadDate { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
}