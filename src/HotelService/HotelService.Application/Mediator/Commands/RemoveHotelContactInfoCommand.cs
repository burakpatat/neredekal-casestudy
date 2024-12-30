using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class RemoveHotelContactInfoCommand : IRequest<bool>
    {
        public Guid ContactInfoId { get; set; }
    }

}
