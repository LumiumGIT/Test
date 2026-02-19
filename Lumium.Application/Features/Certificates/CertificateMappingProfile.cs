using AutoMapper;
using Domain.Entities.Portal;
using Lumium.Application.Features.Certificates.DTOs;

namespace Lumium.Application.Features.Certificates;

public class CertificateMappingProfile : Profile
{
    public CertificateMappingProfile()
    {
        CreateMap<Certificate, CertificateDto>()
            .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));

        CreateMap<CertificateDto, Certificate>()
            .ForMember(dest => dest.Client, opt => opt.Ignore())
            .ForMember(dest => dest.TenantId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}