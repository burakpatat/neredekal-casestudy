using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHotelCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelRepository = _unitOfWork.GetRepository<Hotel>();

            var hotelEntity = await hotelRepository.GetByIdAsync(request.HotelId);
            if (hotelEntity == null) return false;

            await hotelRepository.RemoveAsync(hotelEntity);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
