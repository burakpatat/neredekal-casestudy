using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;

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
            var hotelRepository = _unitOfWork.GetRepository<Hotel>();

            var query = from h in hotelRepository.Table.Include(h => h.ContactInfos)
                        where h.Id == request.HotelId
                        from ci in h.ContactInfos
                        where ci.Type == (HotelContactInfoType)request.Type
                        select new { Hotel = h, ContactInfo = ci };

            var result = await query.FirstOrDefaultAsync(cancellationToken);
            if (result == null) return false;

            result.Hotel.ContactInfos.Remove(result.ContactInfo);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }

}
