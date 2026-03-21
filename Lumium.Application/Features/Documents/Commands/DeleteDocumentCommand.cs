using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Documents.Commands;

public record DeleteDocumentCommand(Guid Id) : IRequest<Result>;

public class DeleteDocumentCommandHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<DeleteDocumentCommand, Result>
{
    public async Task<Result> Handle(
        DeleteDocumentCommand request,
        CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var document = await context.Documents
                    .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

                if (document == null)
                {
                    return Result.Failure("Dokument nije pronađen");
                }

                context.Documents.Remove(document);
                var deletedCount = await context.SaveChangesAsync(cancellationToken);

                return deletedCount == 0
                    ? Result.Failure("Dokument nije obrisan")
                    : Result.Success($"Dokument '{document.Name}' je uspešno obrisan");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri brisanju dokumenta: {ex.Message}");
            }
        }, cancellationToken);
}