using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Common.Models;
using Lumium.Application.Features.Documents.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Documents.Commands;

public record UpdateDocumentCommand(Guid DocumentId, DocumentFormDto DocumentFormDto) : IRequest<Result>;

public class UpdateDocumentCommandHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<UpdateDocumentCommand, Result>
{
    public async Task<Result> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var document = await context.Documents
                .FirstOrDefaultAsync(d => d.Id == request.DocumentId, cancellationToken);

            if (document == null)
            {
                return Result.Failure("Dokument nije pronađen.");
            }

            mapper.Map(request.DocumentFormDto, document);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success("Dokument je uspešno ažuriran.");
        }, cancellationToken);
    }
}