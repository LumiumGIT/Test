using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Clients.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Clients.Queries;

public class GetClientsQuery : IRequest<List<ClientDto>>;

public class GetClientsQueryHandler(IApplicationDbContextFactory contextFactory) : IRequestHandler<GetClientsQuery, List<ClientDto>>
{
    public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.Clients
                .OrderBy(c => c.Name)
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    LegalForm = c.LegalForm,
                    TaxNumber = c.TaxNumber,
                    TaxIdentificationNumber = c.TaxIdentificationNumber,
                    IsPdv = c.IsPdv,
                    ResponsiblePerson = c.ResponsiblePerson,
                    BackupPerson = c.BackupPerson,
                    Address = c.Address,
                    PhoneNumber = c.PhoneNumber,
                    Director = c.Director,
                    Email = c.Email,
                    EcoTax = c.EcoTax,
                    BeneficialOwners = c.BeneficialOwners,
                    Croso = c.Croso,
                    Pep = c.Pep,
                    WingsTemplate = c.WingsTemplate,
                    IsActive = c.IsActive,
                    BusinessActivity = c.BusinessActivity,
                    Country = c.Country,
                    RiskLevel = c.RiskLevel,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }, cancellationToken);
    }
}