using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;

namespace HotelService.Application.Services
{
    public interface IHotelService
    {
        Task<HotelDto> CreateHotelAsync(CreateHotelCommand command);
        Task<bool> DeleteHotelAsync(Guid id);
        Task<HotelContactInfoDto> AddHotelContactInfoAsync(Guid hotelId, HotelContactInfoDto contactInfo);
        Task<bool> RemoveHotelContactInfoAsync(Guid hotelId, int contactInfoType);
        Task<HotelRepresentativeDto> AddHotelRepresentativeAsync(Guid hotelId, HotelRepresentativeDto representative);
        Task<IEnumerable<HotelRepresentativeDto>> GetHotelRepresentativesAsync(Guid hotelId);
        Task<HotelDto> GetHotelDetailByIdAsync(Guid hotelId);
        Task<List<HotelDto>> GetHotelDetailsAsync();
    }

}
