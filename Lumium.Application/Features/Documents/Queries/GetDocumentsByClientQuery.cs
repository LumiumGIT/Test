using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Documents.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Documents.Queries;

public record GetDocumentsByClientQuery(Guid ClientId) : IRequest<List<DocumentDto>>;

public class GetDocumentsByClientQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetDocumentsByClientQuery, List<DocumentDto>>
{
    public async Task<List<DocumentDto>>
        Handle(GetDocumentsByClientQuery request, CancellationToken cancellationToken) =>
        await contextFactory.ExecuteInContextAsync(async context =>
        {
            var documents = await context.Documents
                .Where(d => d.ClientId == request.ClientId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<DocumentDto>>(documents);
        }, cancellationToken);
}