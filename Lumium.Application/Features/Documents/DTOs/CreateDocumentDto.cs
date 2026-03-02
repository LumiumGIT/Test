using Domain.Enums.Documents;

namespace Lumium.Application.Features.Documents.DTOs;

public class CreateDocumentDto
{
    public Guid? ClientId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public DocumentCategory Category { get; set; } = DocumentCategory.Other;
    public string Url { get; set; } = string.Empty;
    public string? Description { get; set; }
}