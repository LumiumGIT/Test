using AutoMapper;
using Domain.Enums.Clients;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.ClientContacts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.ClientContacts.Commands;

public record UpdateClientContactCommand(Guid ContactId, ClientContactFormDto ClientContactFormDto) : IRequest<Result>;

public class UpdateClientContactCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<UpdateClientContactCommand, Result>
{
    public async Task<Result> Handle(UpdateClientContactCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var contact = await context.ClientContacts
                .FirstOrDefaultAsync(c => c.Id == request.ContactId, cancellationToken);

            if (contact is null)
            {
                return Result.Failure("Kontakt nije pronađen.");
            }

            if (request.ClientContactFormDto.Type == ContactType.Primary && contact.Type != ContactType.Primary)
            {
                var primaryExists = await context.ClientContacts
                    .AnyAsync(c => c.ClientId == request.ClientContactFormDto.ClientId
                                   && c.Type == ContactType.Primary
                                   && c.Id != request.ContactId, cancellationToken);

                if (primaryExists)
                {
                    return Result.Failure("Klijent već ima glavni kontakt.");
                }
            }

            mapper.Map(request.ClientContactFormDto, contact);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Kontakt je uspešno izmenjen.");
        }, cancellationToken);
    }
}