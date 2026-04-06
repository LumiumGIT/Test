using Domain.Enums.Clients;

namespace Lumium.Application.Features.ClientContacts.DTOs;

public class ClientContactFormDto
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Description { get; set; }
    public ContactType Type { get; set; } = ContactType.Secondary;
}