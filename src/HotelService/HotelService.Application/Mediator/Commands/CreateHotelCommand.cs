using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class CreateHotelCommand : IRequest<HotelDto>
    {
        public string Name { get; set; }
        public ICollection<HotelRepresentativeDto> Representatives { get; set; }
        public ICollection<HotelContactInfoDto> ContactInfos { get; set; }
    }
}
