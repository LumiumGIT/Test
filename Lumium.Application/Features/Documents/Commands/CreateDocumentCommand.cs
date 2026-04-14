using AutoMapper;
using Domain.Entities.Portal;
using Domain.Entities.Portal.Tenant;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Documents.DTOs;
using MediatR;

namespace Lumium.Application.Features.Documents.Commands;

public record CreateDocumentCommand(DocumentFormDto DocumentFormDto) : IRequest<Result>;

public class CreateDocumentCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<CreateDocumentCommand, Result>
{
    public async Task<Result> Handle(CreateDocumentCommand request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            try
            {
                var newDocument = mapper.Map<Document>(request.DocumentFormDto);
                newDocument.UploadedAt = DateTime.UtcNow;

                context.Documents.Add(newDocument);
                var savedCount = await context.SaveChangesAsync(cancellationToken);

                return savedCount == 0
                    ? Result.Failure("Dokument nije sačuvan u bazi")
                    : Result.Success($"Dokument '{newDocument.Name}' je uspešno dodat");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Greška pri dodavanju dokumenta: {ex.Message}");
            }
        }, cancellationToken);
}