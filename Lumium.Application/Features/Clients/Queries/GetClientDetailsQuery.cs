using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Queries;

public record GetClientDetailsQuery(Guid ClientId) : IRequest<ClientDetailsDto?>;

public class GetClientDetailsQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetClientDetailsQuery, ClientDetailsDto?>
{
    public async Task<ClientDetailsDto?> Handle(GetClientDetailsQuery request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            var client = await context.Clients
                .Include(c => c.Certificates)
                .Include(c => c.Contracts)
                .Include(c => c.Documents)
                .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

            if (client == null)
            {
                return null;
            }

            var regulatoryBodies = await context.RegulatoryBodies
                .ToDictionaryAsync(r => r.Id, r => r.Name, cancellationToken);

            var clientDto = mapper.Map<ClientDetailsDto>(client);

            foreach (var cert in clientDto.Certificates.Where(certificate =>
                         regulatoryBodies.ContainsKey(certificate.RegulatoryBodyId)))
            {
                cert.RegulatoryBodyName = regulatoryBodies[cert.RegulatoryBodyId];
            }

            return clientDto;
        }, cancellationToken);
}