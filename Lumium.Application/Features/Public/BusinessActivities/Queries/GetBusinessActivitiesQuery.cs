using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Public.BusinessActivities.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Public.BusinessActivities.Queries;

public record GetBusinessActivitiesQuery : IRequest<List<BusinessActivityDto>>;

public class GetBusinessActivitiesQueryHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<GetBusinessActivitiesQuery, List<BusinessActivityDto>>
{
    public async Task<List<BusinessActivityDto>> Handle(GetBusinessActivitiesQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.BusinessActivities
                .Where(b => b.Valid)
                .OrderBy(b => b.Name)
                .Select(b => new BusinessActivityDto(b.Id, b.Name))
                .ToListAsync(cancellationToken);
        }, cancellationToken);
    }
}