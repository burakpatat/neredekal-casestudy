using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class RemoveHotelContactInfoCommand : IRequest<bool>
    {
        public Guid HotelId { get; set; }
        public int Type { get; set; }
    }

}
