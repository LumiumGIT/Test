using Domain.Entities.Portal.Public;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Public.Countries.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Public.Countries.Queries;

public record GetCountriesQuery : IRequest<List<CountryDto>>;

public class GetCountriesQueryHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<GetCountriesQuery, List<CountryDto>>
{
    public async Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.Countries
                .OrderBy(c => c.Name)
                .Select(c => new CountryDto(c.Id, c.Name, c.IsoCode))
                .ToListAsync(cancellationToken);
        }, cancellationToken);
    }
}