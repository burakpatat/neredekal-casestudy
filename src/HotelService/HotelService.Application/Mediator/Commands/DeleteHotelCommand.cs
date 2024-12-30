
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class DeleteHotelCommand : IRequest<bool>
    {
        public Guid HotelId { get; set; }
    }
}
