using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Certificates.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Certificates.Commands;

public record UpdateCertificateCommand(Guid CertificateId, CertificateFormDto CertificateFormDto) : IRequest<Result>;

public class UpdateCertificateCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<UpdateCertificateCommand, Result>
{
    public async Task<Result> Handle(UpdateCertificateCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var certificate = await context.Certificates
                .FirstOrDefaultAsync(c => c.Id == request.CertificateId, cancellationToken);

            if (certificate == null)
            {
                return Result.Failure("Sertifikat nije pronađen.");
            }

            mapper.Map(request.CertificateFormDto,  certificate);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Sertifikat je uspešno ažuriran.");
        }, cancellationToken);
    }
}