using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class DeleteHotelRepresentativeCommand : IRequest<bool>
    {
        public Guid RepresentativeId { get; set; }
    }
}
