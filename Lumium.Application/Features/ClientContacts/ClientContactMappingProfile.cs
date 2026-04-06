using AutoMapper;
using Domain.Entities.Portal.Tenant;
using Lumium.Application.Features.ClientContacts.DTOs;

namespace Lumium.Application.Features.ClientContacts;

public class ClientContactMappingProfile : Profile
{
    public ClientContactMappingProfile()
    {
        CreateMap<ClientContact, ClientContactDto>();

        CreateMap<ClientContactFormDto, ClientContact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedByUser, opt => opt.Ignore());
    }
}