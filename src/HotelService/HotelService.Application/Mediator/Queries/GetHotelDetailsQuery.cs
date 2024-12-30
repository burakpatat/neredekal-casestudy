using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.Mediator.Queries
{
    public class GetHotelDetailsQuery : IRequest<HotelDto>
    {
        public Guid HotelId { get; set; }
    }

}
