using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;

namespace Lumium.Application.Features.Clients.Commands;

public record CreateClientCommand(ClientDto ClientDto) : IRequest<Result>;

public class CreateClientCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<CreateClientCommand, Result>
{
    public async Task<Result> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var newClient = mapper.Map<Client>(request.ClientDto);
                newClient.Id = Guid.NewGuid();

                context.Clients.Add(newClient);
                var savedCount = await context.SaveChangesAsync(cancellationToken);

                return savedCount == 0
                    ? Result.Failure("Klijent nije sačuvan u bazi")
                    : Result.Success($"Klijent '{newClient.Name}' je uspešno kreiran");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri kreiranju klijenta: {ex.Message}");
            }
        }, cancellationToken);
    }
}