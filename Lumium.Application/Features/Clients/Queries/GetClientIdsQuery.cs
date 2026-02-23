using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Queries;

public class GetClientIdsQuery : IRequest<List<(Guid Id, string Name)>>;

public class GetClientIdsQueryHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<GetClientIdsQuery, List<(Guid Id, string Name)>>
{
    public async Task<List<(Guid Id, string Name)>> Handle(GetClientIdsQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.Clients
                .OrderBy(c => c.Name)
                .Select(c => new ValueTuple<Guid, string>(c.Id, c.Name))
                .ToListAsync(cancellationToken);
        }, cancellationToken);
    }
}