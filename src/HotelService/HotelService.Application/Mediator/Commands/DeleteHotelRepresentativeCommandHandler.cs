using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class DeleteHotelRepresentativeCommandHandler : IRequestHandler<DeleteHotelRepresentativeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteHotelRepresentativeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteHotelRepresentativeCommand request, CancellationToken cancellationToken)
        {
            var representativeRepository = _unitOfWork.GetRepository<HotelRepresentative>();
            var representative = await representativeRepository.GetByIdAsync(request.RepresentativeId);

            if (representative == null) return false;

            await representativeRepository.RemoveAsync(representative);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
