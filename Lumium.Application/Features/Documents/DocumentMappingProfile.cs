using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Features.Documents.DTOs;

namespace Lumium.Application.Features.Documents;

public class DocumentMappingProfile : Profile
{
    public DocumentMappingProfile()
    {
        CreateMap<Document, DocumentDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));

        CreateMap<DocumentFormDto, Document>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.SelectedClient.Id));
    }
}