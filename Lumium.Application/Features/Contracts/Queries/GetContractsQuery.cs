using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Contracts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Contracts.Queries;

public record GetContractsQuery : IRequest<List<ContractDto>>;

public class GetContractsQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetContractsQuery, List<ContractDto>>
{
    public async Task<List<ContractDto>> Handle(
        GetContractsQuery request,
        CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contracts = await context.Contracts
                .Include(c => c.Client)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<ContractDto>>(contracts);
        }, cancellationToken);
    }
}