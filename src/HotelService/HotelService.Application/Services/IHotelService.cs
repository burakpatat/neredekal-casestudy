using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;

namespace HotelService.Application.Services
{
    public interface IHotelService
    {
        Task<HotelDto> CreateHotelAsync(CreateHotelCommand command);
        Task<bool> DeleteHotelAsync(Guid id);
        Task<HotelContactInfoDto> AddHotelContactInfoAsync(Guid hotelId, HotelContactInfoDto contactInfo);
        Task<bool> RemoveHotelContactInfoAsync(Guid contactInfoId);
        Task<IEnumerable<HotelRepresentativeDto>> GetHotelRepresentativesAsync(Guid hotelId);
        Task<HotelDto> GetHotelDetailsAsync(Guid hotelId);
    }
}
