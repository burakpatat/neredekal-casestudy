using HotelService.Application.DTOs;
using MediatR;


namespace HotelService.Application.Mediator.Queries
{
    public class GetHotelRepresentativesQuery : IRequest<List<HotelRepresentativeDto>>
    {
        public Guid HotelId { get; set; }
    }

}
