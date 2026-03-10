using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Certificates.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Certificates.Queries;

public record GetCertificatesQuery : IRequest<List<CertificateDto>>;

public class GetCertificatesQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetCertificatesQuery, List<CertificateDto>>
{
    public async Task<List<CertificateDto>> Handle(GetCertificatesQuery request, CancellationToken cancellationToken)
    {
         return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var certificates = await context.Certificates
                .Include(c => c.Client)
                .OrderBy(c => c.Client.Name)
                .ToListAsync(cancellationToken);
            
            var regulatoryBodies = await context.RegulatoryBodies
                .ToDictionaryAsync(r => r.Id, r => r.Name, cancellationToken);
            
            var certificateDtOs = mapper.Map<List<CertificateDto>>(certificates);

            foreach (var certificate in certificateDtOs.Where(certificate =>
                         regulatoryBodies.ContainsKey(certificate.RegulatoryBodyId)))
            {
                certificate.RegulatoryBodyName = regulatoryBodies[certificate.RegulatoryBodyId];
            }

            return certificateDtOs;
        }, cancellationToken);
    }
}