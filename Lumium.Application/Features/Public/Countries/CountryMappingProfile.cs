using AutoMapper;
using Domain.Entities.Portal.Public;
using Lumium.Application.Features.Public.Countries.DTOs;

namespace Lumium.Application.Features.Public.Countries;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        CreateMap<Country, CountryDto>();
    }
}