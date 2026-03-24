using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Commands;

public record DeleteClientCommand(Guid Id) : IRequest<Result>;

public class DeleteClientsCommandHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<DeleteClientCommand, Result>
{
    public async Task<Result> Handle(DeleteClientCommand request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var client = await context.Clients.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (client == null)
                {
                    return Result.Failure("Klijent nije pronađen");
                }

                context.Clients.Remove(client);
                
                var deletedCount = await context.SaveChangesAsync(cancellationToken);

                return deletedCount == 0
                    ? Result.Failure("Klijent nije obrisan")
                    : Result.Success($"Klijent {client.Name} je uspešno obrisan");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri brisanju klijenta: {ex.Message}");
            }
        }, cancellationToken);
}