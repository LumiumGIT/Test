using AutoMapper;
using Domain.Entities.Portal;
using Domain.Entities.Portal.Tenant;
using Domain.Enums.Contracts;
using Lumium.Application.Features.Contracts.DTOs;

namespace Lumium.Application.Features.Contracts;

public class ContractMappingProfile : Profile
{
    public ContractMappingProfile()
    {
        CreateMap<Contract, ContractDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name))
            .ForMember(dest => dest.Annexes, opt => opt.MapFrom(src => src.Annexes));

        CreateMap<ContractFormDto, Contract>()
            .ForMember(dest => dest.ClientId, opt => opt.MapFrom(src => src.SelectedClient.Id))
            .ForMember(dest => dest.ParentContractId, opt => opt.MapFrom(src =>
                src.Kind == ContractKind.Annex ? src.SelectedParentContract.Id : (Guid?)null))
            .ForMember(dest => dest.Annexes, opt => opt.Ignore());
    }
}