using AutoMapper;
using Domain.Entities.Portal.Tenant;
using Domain.Enums.Clients;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.ClientContacts.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.ClientContacts.Commands;

public record CreateClientContactCommand(ClientContactFormDto Model) : IRequest<Result>;

public class CreateClientContactCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<CreateClientContactCommand, Result>
{
    public async Task<Result> Handle(CreateClientContactCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            if (request.Model.Type == ContactType.Primary)
            {
                var primaryExists = await context.ClientContacts
                    .AnyAsync(c => c.ClientId == request.Model.ClientId 
                                   && c.Type == ContactType.Primary, cancellationToken);

                if (primaryExists)
                {
                    return Result.Failure("Klijent već ima primarni kontakt.");
                }
            }

            var contact = mapper.Map<ClientContact>(request.Model);

            await context.ClientContacts.AddAsync(contact, cancellationToken);
            var savedCount = await context.SaveChangesAsync(cancellationToken);

            return savedCount == 0
                ? Result.Failure("Kontakt nije sačuvan u bazi")
                : Result.Success($"Kontakt '{contact.Name}' je uspešno kreiran");
        }, cancellationToken);
    }
}