using HotelService.Application.DTOs;
using MediatR;
using SharedKernel.Enums;


namespace HotelService.Application.Mediator.Commands
{
    public class AddHotelContactInfoCommand : IRequest<HotelContactInfoDto>
    {
        public Guid HotelId { get; set; }
        public HotelContactInfoType Type { get; set; }
        public string Value { get; set; }
    }
}
