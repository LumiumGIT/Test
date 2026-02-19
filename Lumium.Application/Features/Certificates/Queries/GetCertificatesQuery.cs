using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Certificates.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Certificates.Queries;

public class GetCertificatesQuery : IRequest<Result<List<CertificateDto>>>;

public class GetCertificatesQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetCertificatesQuery, Result<List<CertificateDto>>>
{
    public async Task<Result<List<CertificateDto>>> Handle(
        GetCertificatesQuery request,
        CancellationToken cancellationToken)
    {
        var certificates = await contextFactory.ExecuteInContextAsync(async context =>
        {
            return await context.Certificates
                .Include(c => c.Client)
                .OrderBy(c => c.ExpiryDate)
                .ToListAsync(cancellationToken);
        }, cancellationToken);

        return Result<List<CertificateDto>>.Success(mapper.Map<List<CertificateDto>>(certificates).ToList());
    }
}