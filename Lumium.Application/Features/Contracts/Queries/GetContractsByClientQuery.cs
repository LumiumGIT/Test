using AutoMapper;
using Domain.Enums.Contracts;
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
    public async Task<List<ContractDto>>
        Handle(GetContractsByClientQuery request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contracts = await context.Contracts
                .Include(c => c.Annexes)
                .Where(c => c.ClientId == request.ClientId && c.Kind == ContractKind.Main)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<ContractDto>>(contracts);
        }, cancellationToken);
}