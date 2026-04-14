using AutoMapper;
using Domain.Entities.Portal.Public;
using Lumium.Application.Features.Public.BusinessActivities.DTOs;

namespace Lumium.Application.Features.Public.BusinessActivities;

public class BusinessActivityMappingProfile :  Profile
{
    public BusinessActivityMappingProfile()
    {
        CreateMap<BusinessActivity, BusinessActivityDto>();
    }
}