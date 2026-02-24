using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Commands;

public record DeleteClientsCommand(HashSet<Guid> ClientIds) : IRequest<Result<int>>;
public class DeleteClientsCommandHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<DeleteClientsCommand, Result<int>>
{
    public async Task<Result<int>> Handle(DeleteClientsCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                if (request.ClientIds.Count == 0)
                {
                    return Result<int>.Failure("Nema selektovanih klijenata");
                }

                var clientsToDelete = await context.Clients
                    .Where(c => request.ClientIds.Contains(c.Id))
                    .ToListAsync(cancellationToken);

                if (clientsToDelete.Count == 0)
                {
                    return Result<int>.Failure("Klijenti nisu pronađeni");
                }

                context.Clients.RemoveRange(clientsToDelete);
                var deletedCount = await context.SaveChangesAsync(cancellationToken);

                return deletedCount == 0
                    ? Result<int>.Failure("Klijenti nisu obrisani")
                    : Result<int>.Success(deletedCount, $"Uspešno obrisano {deletedCount} klijent(a)");
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Greška pri brisanju: {ex.Message}");
            }
        }, cancellationToken);
    }
}