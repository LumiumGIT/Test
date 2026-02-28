using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Features.Contracts.DTOs;

namespace Lumium.Application.Features.Contracts;

public class ContractMappingProfile : Profile
{
    public ContractMappingProfile()
    {
        CreateMap<Contract, ContractDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));
        
        CreateMap<CreateContractDto, Contract>();
    }
}