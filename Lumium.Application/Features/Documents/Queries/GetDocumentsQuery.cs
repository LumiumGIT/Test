using AutoMapper;
using Lumium.Application.Common.Extensions;
using Lumium.Application.Common.Interfaces;
using Lumium.Application.Features.Documents.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Lumium.Application.Features.Documents.Queries;

public class GetDocumentsQuery : IRequest<List<DocumentDto>>;

public class GetDocumentsQueryHandler(IApplicationDbContextFactory contextFactory, IMapper mapper)
    : IRequestHandler<GetDocumentsQuery, List<DocumentDto>>
{
    public async Task<List<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await contextFactory.ExecuteInContextAsync(async context =>
        {
            var documents = await context.Documents
                .Include(d => d.Client)
                .OrderBy(d => d.Client.Name)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<DocumentDto>>(documents);
        }, cancellationToken);
    }
}