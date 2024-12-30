using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;
using HotelService.Application.Mediator.Queries;
using MediatR;

namespace HotelService.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IMediator _mediator;

        public HotelService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<HotelDto> CreateHotelAsync(CreateHotelCommand command)
        {
            var hotelDto = await _mediator.Send(command);
            return hotelDto;
        }

        public async Task<HotelRepresentativeDto> AddHotelRepresentativeAsync(Guid hotelId, HotelRepresentativeDto representative)
        {
            var command = new AddHotelRepresentativeCommand
            {
                HotelId = hotelId,
                Name = representative.Name,
                SurName = representative.SurName
            };

            return await _mediator.Send(command);
        }

        public async Task<bool> DeleteHotelAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteHotelCommand { HotelId = id });
            return result;
        }

        public async Task<HotelContactInfoDto> AddHotelContactInfoAsync(Guid hotelId, HotelContactInfoDto contactInfo)
        {
            var command = new AddHotelContactInfoCommand
            {
                HotelId = hotelId,
                Type = contactInfo.Type,
                Value = contactInfo.Value
            };
            return await _mediator.Send(command);
        }

        public async Task<bool> RemoveHotelContactInfoAsync(Guid contactInfoId)
        {
            var command = new RemoveHotelContactInfoCommand { ContactInfoId = contactInfoId };
            return await _mediator.Send(command);
        }

        public async Task<IEnumerable<HotelRepresentativeDto>> GetHotelRepresentativesAsync(Guid hotelId)
        {
            var query = new GetHotelRepresentativesQuery { HotelId = hotelId };
            return await _mediator.Send(query);
        }

        public async Task<HotelDto> GetHotelDetailsAsync(Guid hotelId)
        {
            var query = new GetHotelDetailsQuery { HotelId = hotelId };
            return await _mediator.Send(query);
        }
    }


}
