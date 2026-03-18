using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Certificates.Commands;

public record DeleteCertificateCommand(Guid Id) : IRequest<Result>;

public class DeleteCertificateCommandHandler(IApplicationDbContextFactory contextFactory)
    : IRequestHandler<DeleteCertificateCommand, Result>
{
    public async Task<Result> Handle(
        DeleteCertificateCommand request,
        CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var certificate = await context.Certificates
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (certificate == null)
                {
                    return Result.Failure("Sertifikat nije pronađen");
                }

                context.Certificates.Remove(certificate);
                var deletedCount = await context.SaveChangesAsync(cancellationToken);

                return deletedCount == 0
                    ? Result.Failure("Sertifikat nije obrisan")
                    : Result.Success($"Sertifikat '{certificate.CertificateName}' je uspešno obrisan");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri brisanju sertifikata: {ex.Message}");
            }
        }, cancellationToken);
    }
}