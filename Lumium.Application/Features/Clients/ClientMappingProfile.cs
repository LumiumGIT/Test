using AutoMapper;
using Domain.Entities.Portal;
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

        CreateMap<Client, ClientDto>();
    }
}