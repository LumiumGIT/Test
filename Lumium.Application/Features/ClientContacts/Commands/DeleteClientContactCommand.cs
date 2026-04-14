using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.ClientContacts.Commands;

public record DeleteClientContactCommand(Guid Id) : IRequest<Result>;

public class DeleteClientContactCommandHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<DeleteClientContactCommand, Result>
{
    public async Task<Result> Handle(DeleteClientContactCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contact = await context.ClientContacts
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (contact is null)
            {
                return Result.Failure("Kontakt nije pronađen.");
            }

            context.ClientContacts.Remove(contact);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Kontakt je uspešno obrisan.");
        }, cancellationToken);
    }
}