using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Contracts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Contracts.Commands;

public record UpdateContractCommand(Guid ContractId, ContractFormDto ContractFormDto) : IRequest<Result>;

public class UpdateContractCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<UpdateContractCommand, Result>
{
    public async Task<Result> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contract = await context.Contracts
                .FirstOrDefaultAsync(c => c.Id == request.ContractId, cancellationToken);

            if (contract == null)
            {
                return Result.Failure("Ugovor nije pronađen.");
            }
            
            mapper.Map(request.ContractFormDto,  contract);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Ugovor je uspešno ažuriran.");
        }, cancellationToken);
    }
}