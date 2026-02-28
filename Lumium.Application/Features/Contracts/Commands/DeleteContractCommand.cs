using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Contracts.Commands;

public record DeleteContractCommand(Guid Id) : IRequest<Result>;

public class DeleteContractCommandHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<DeleteContractCommand, Result>
{
    public async Task<Result> Handle(DeleteContractCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var contract = await context.Contracts.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (contract == null)
                {
                    return Result.Failure("Ugovor nije pronađen");
                }

                context.Contracts.Remove(contract);
                var deletedCount = await context.SaveChangesAsync(cancellationToken);

                return deletedCount == 0
                    ? Result.Failure("Ugovor nije obrisan")
                    : Result.Success($"Ugovor '{contract.ContractNumber}' je uspešno obrisan");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri brisanju ugovora: {ex.Message}");
            }
        }, cancellationToken);
    }
}