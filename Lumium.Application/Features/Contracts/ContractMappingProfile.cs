using AutoMapper;
using Domain.Entities.Portal;
using Domain.Entities.Portal.Tenant;
using Lumium.Application.Features.Contracts.DTOs;

namespace Lumium.Application.Features.Contracts;

public class ContractMappingProfile : Profile
{
    public ContractMappingProfile()
    {
        CreateMap<Contract, ContractDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));

        CreateMap<ContractFormDto, Contract>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.SelectedClient.Id));
    }
}