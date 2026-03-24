using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Commands;

public record UpdateClientCommand(Guid ClientId, ClientFormDto ClientFormDto) : IRequest<Result>;

public class UpdateClientCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<UpdateClientCommand, Result>
{
    public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

            if (client == null)
            {
                return Result.Failure("Klijent nije pronađen.");
            }

            mapper.Map(request.ClientFormDto, client);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Klijent je uspešno ažuriran.");
        }, cancellationToken);
    }
}