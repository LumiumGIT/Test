using AutoMapper;
using Domain.Entities.Portal;
using Domain.Entities.Portal.Tenant;
using Lumium.Application.Features.Clients.DTOs;

namespace Lumium.Application.Features.Clients;

public class ClientMappingProfile : Profile
{
    public ClientMappingProfile()
    {
        CreateMap<ClientDto, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<Client, ClientDto>()
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.BusinessActivity, opt => opt.MapFrom(src => src.BusinessActivity));

        CreateMap<Client, ClientDetailsDto>()
            .ForMember(dest => dest.Certificates, opt => opt.MapFrom(src => src.Certificates))
            .ForMember(dest => dest.Contracts, opt => opt.MapFrom(src => src.Contracts))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents))
            .ForMember(dest => dest.BusinessActivity, opt => opt.MapFrom(src => src.BusinessActivity))
            .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country));

        CreateMap<ClientFormDto, Client>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId, opt => opt.Ignore())
            .ForMember(dest => dest.Certificates, opt => opt.Ignore())
            .ForMember(dest => dest.Contracts, opt => opt.Ignore())
            .ForMember(dest => dest.Documents, opt => opt.Ignore());

        CreateMap<ClientDetailsDto, ClientDto>()
            .ForMember(dest => dest.BusinessActivityId, opt => opt.MapFrom(src => src.BusinessActivity!.Id))
            .ForMember(dest => dest.CountryId, opt => opt.MapFrom(src => src.Country!.Id));
    }
}