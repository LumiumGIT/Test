using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Contracts.DTOs;
using MediatR;

namespace Lumium.Application.Features.Contracts.Commands;

public record CreateContractCommand(CreateContractDto ContractDto) : IRequest<Result>;

public class CreateContractCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<CreateContractCommand, Result>
{
    public async Task<Result> Handle(
        CreateContractCommand request,
        CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var newContract = mapper.Map<Contract>(request.ContractDto);
                newContract.Id = Guid.NewGuid();

                context.Contracts.Add(newContract);
                var savedCount = await context.SaveChangesAsync(cancellationToken);

                return savedCount == 0
                    ? Result.Failure("Ugovor nije sačuvan u bazi")
                    : Result.Success($"Ugovor '{newContract.ContractNumber}' je uspešno kreiran");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri kreiranju ugovora: {ex.Message}");
            }
        }, cancellationToken);
    }
}