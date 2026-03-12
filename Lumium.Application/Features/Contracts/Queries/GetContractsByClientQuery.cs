using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Contracts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Contracts.Queries;

public record GetContractsByClientQuery(Guid ClientId) : IRequest<List<ContractDto>>;

public class GetContractsByClientQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetContractsByClientQuery, List<ContractDto>>
{
    public async Task<List<ContractDto>> Handle(GetContractsByClientQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contracts = await context.Contracts
                .Where(c => c.ClientId == request.ClientId)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<ContractDto>>(contracts);
        }, cancellationToken);
    }
}