using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;

namespace HotelService.Application.Mapping
{
    public class HotelMapping : Profile
    {
        public HotelMapping()
        {
            // Hotel entity -> HotelDto
            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.Representatives, opt => opt.MapFrom(src => src.Representatives))
                .ForMember(dest => dest.ContactInfos, opt => opt.MapFrom(src => src.ContactInfos));

            // HotelRepresentative entity -> HotelRepresentativeDto
            CreateMap<HotelRepresentative, HotelRepresentativeDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

            // HotelContactInfo entity -> HotelContactInfoDto
            CreateMap<HotelContactInfo, HotelContactInfoDto>();
        }
    }
}
