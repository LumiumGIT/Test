using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;

namespace Lumium.Application.Features.Clients.Commands;

public class CreateClientCommand(ClientDto clientDto) : IRequest<bool>
{
    public ClientDto ClientDto { get; } = clientDto;
}

public class CreateClientCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<CreateClientCommand, bool>
{
    public async Task<bool> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var newClient = mapper.Map<Client>(request.ClientDto);
        
        newClient.Id = Guid.NewGuid();
        
        context.Clients.Add(newClient);

        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}