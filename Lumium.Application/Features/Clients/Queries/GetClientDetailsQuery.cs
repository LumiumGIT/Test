using AutoMapper;
using Domain.Entities.Portal.Public;
using Domain.Enums.Clients;
using Domain.Enums.Contracts;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Queries;

public record GetClientDetailsQuery(Guid ClientId) : IRequest<ClientDetailsDto?>;

public class GetClientDetailsQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetClientDetailsQuery, ClientDetailsDto?>
{
    public async Task<ClientDetailsDto?> Handle(GetClientDetailsQuery request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            var client = await context.Clients
                .Include(c => c.Contracts.Where(contract => contract.Status == ContractStatus.Active))
                .Include(c => c.Certificates.Where(certificate => certificate.ExpiryDate > DateTime.Now))
                .Include(c => c.Documents)
                .Include(c => c.Country)
                .Include(c => c.BusinessActivity)
                .Include(c => c.Contacts.Where(contact => contact.Type == ContactType.Primary))
                .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

            if (client == null)
            {
                return null;
            }

            var clientDto = mapper.Map<ClientDetailsDto>(client);
            
            return clientDto;
        }, cancellationToken);
}