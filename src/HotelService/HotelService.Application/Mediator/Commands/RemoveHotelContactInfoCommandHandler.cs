using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class RemoveHotelContactInfoCommandHandler : IRequestHandler<RemoveHotelContactInfoCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveHotelContactInfoCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RemoveHotelContactInfoCommand request, CancellationToken cancellationToken)
        {
            var contactInfoRepository = _unitOfWork.GetRepository<HotelContactInfo>();

            var contactInfo = await contactInfoRepository.GetByIdAsync(request.ContactInfoId);
            if (contactInfo == null) return false;

            await contactInfoRepository.RemoveAsync(contactInfo);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
