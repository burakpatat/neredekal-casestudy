using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class AddHotelRepresentativeCommand : IRequest<HotelRepresentativeDto>
    {
        public Guid HotelId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
    }
}
