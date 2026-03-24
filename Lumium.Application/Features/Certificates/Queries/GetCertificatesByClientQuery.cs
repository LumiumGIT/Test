using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Certificates.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Certificates.Queries;

public record GetCertificatesByClientQuery(Guid ClientId) : IRequest<List<CertificateDto>>;

public class GetCertificatesByClientQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetCertificatesByClientQuery, List<CertificateDto>>
{
    public async Task<List<CertificateDto>> Handle(GetCertificatesByClientQuery request,
        CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            var certificates = await context.Certificates
                .Where(c => c.ClientId == request.ClientId)
                .OrderByDescending(c => c.ExpiryDate)
                .ToListAsync(cancellationToken);

            var regulatoryBodies = await context.RegulatoryBodies
                .ToDictionaryAsync(r => r.Id, r => r.Name, cancellationToken);

            var certificateDtos = mapper.Map<List<CertificateDto>>(certificates);

            foreach (var certificate in certificateDtos.Where(c => regulatoryBodies.ContainsKey(c.RegulatoryBodyId)))
            {
                certificate.RegulatoryBodyName = regulatoryBodies[certificate.RegulatoryBodyId];
            }

            return certificateDtos;
        }, cancellationToken);
}