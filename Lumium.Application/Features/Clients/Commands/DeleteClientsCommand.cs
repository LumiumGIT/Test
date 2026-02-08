using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Commands;

public class DeleteClientsCommand(HashSet<Guid> clientIds) : IRequest<Result<int>>
{
    public HashSet<Guid> ClientIds { get; } = clientIds;
}

public class DeleteClientsCommandHandler(IApplicationDbContext context)
    : IRequestHandler<DeleteClientsCommand, Result<int>>
{
    public async Task<Result<int>> Handle(
        DeleteClientsCommand request,
        CancellationToken cancellationToken)
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

            // Opciona validacija - proveri da li imaju povezane podatke
            // var hasInvoices = await context.Invoices
            //     .AnyAsync(i => request.ClientIds.Contains(i.ClientId), cancellationToken);
            // if (hasInvoices)
            // {
            //     return Result<int>.Failure("Neki klijenti imaju povezane fakture i ne mogu biti obrisani");
            // }

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
    }
}