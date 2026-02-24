using AutoMapper;
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
    public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.Clients
                .OrderBy(c => c.Name)
                .Select(c => mapper.Map(c, new ClientDto()))
                .ToListAsync(cancellationToken);
        }, cancellationToken);
    }
}