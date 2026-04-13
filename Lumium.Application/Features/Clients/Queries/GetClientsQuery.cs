using AutoMapper;
using Domain.Enums.Clients;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Queries;

public record GetClientsQuery : IRequest<List<ClientDto>>;

public class GetClientsQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetClientsQuery, List<ClientDto>>
{
    public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            var clients = await context.Clients
                .Include(c => c.Country)
                .Include(c => c.BusinessActivity)
                .Include(c => c.Contacts.Where(contact => contact.Type == ContactType.Primary))
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);

            return clients.Select(c => mapper.Map(c, new ClientDto())).ToList();
        }, cancellationToken);
}