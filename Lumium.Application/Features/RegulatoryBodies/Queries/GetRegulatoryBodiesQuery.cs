using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.RegulatoryBodies.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.RegulatoryBodies.Queries;

public record GetRegulatoryBodiesQuery() : IRequest<List<RegulatoryBodyDto>>;

public class GetRegulatoryBodiesQueryHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<GetRegulatoryBodiesQuery, List<RegulatoryBodyDto>>
{
    public async Task<List<RegulatoryBodyDto>> Handle(
        GetRegulatoryBodiesQuery request,
        CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.RegulatoryBodies
                .OrderBy(r => r.Name)
                .Select(r => new RegulatoryBodyDto { Id = r.Id, Name = r.Name })
                .ToListAsync(cancellationToken);
        }, cancellationToken);
    }
}