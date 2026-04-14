using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.ClientContacts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.ClientContacts.Queries;

public record GetClientContactsQuery(Guid ClientId) : IRequest<List<ClientContactDto>>;

public class GetClientContactsQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetClientContactsQuery, List<ClientContactDto>>
{
    public async Task<List<ClientContactDto>> Handle(GetClientContactsQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contacts = await context.ClientContacts
                .Where(c => c.ClientId == request.ClientId)
                .OrderBy(c => c.Type)
                .ThenBy(c => c.Name)
                .ToListAsync(cancellationToken);

            return contacts.Select(mapper.Map<ClientContactDto>).ToList();
        }, cancellationToken);
    }
}