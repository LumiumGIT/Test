using AutoMapper;
using Domain.Entities.Portal;
using Domain.Entities.Portal.Tenant;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Certificates.DTOs;
using MediatR;

namespace Lumium.Application.Features.Certificates.Commands;

public record CreateCertificateCommand(CertificateFormDto CertificateFormDto) : IRequest<Result>;

public class CreateCertificateCommandHandler(
    IApplicationDbContextFactory contextFactory,
    IMapper mapper)
    : IRequestHandler<CreateCertificateCommand, Result>
{
    public async Task<Result> Handle(CreateCertificateCommand request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var newCertificate = mapper.Map<Certificate>(request.CertificateFormDto);

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
        }, cancellationToken);
}