using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Certificates.DTOs;
using MediatR;

namespace Lumium.Application.Features.Certificates.Commands;

public class CreateCertificateCommand(CertificateDto certificateDto) : IRequest<Result>
{
    public CertificateDto CertificateDto { get; } = certificateDto;
}

public class CreateCertificateCommandHandler(IApplicationDbContext context, IMapper mapper, ITenantContext tenantContext)
    : IRequestHandler<CreateCertificateCommand, Result>
{
    public async Task<Result> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (!tenantContext.IsResolved)
            {
                return Result.Failure("Tenant kontekst nije setovan");
            }
        
            var newCertificate = mapper.Map<Certificate>(request.CertificateDto);

            context.Certificates.Add(newCertificate);
            var savedCount = await context.SaveChangesAsync(cancellationToken);

            return savedCount == 0
                ? Result.Failure("Sertifikat nije sačuvan u bazi")
                : Result.Success($"Sertifikat '{newCertificate.CertificateName}' je uspešno kreiran");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Greška pri kreiranju sertifikata: {ex.Message}");
        }
    }
}