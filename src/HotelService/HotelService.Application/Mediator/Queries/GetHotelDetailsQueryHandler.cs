using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.Persistence;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Application.Mediator.Queries
{
    public class GetHotelDetailsQueryHandler : IRequestHandler<GetHotelDetailsQueryById, HotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHotelDetailsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HotelDto> Handle(GetHotelDetailsQueryById request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Hotel>();
            var table = repository.Table;

            var hotelEntity = await table
                .Include(h => h.Representatives)
                .Include(h => h.ContactInfos)
                .FirstOrDefaultAsync(h => h.Id == request.HotelId, cancellationToken);

            if (hotelEntity == null)
                throw new Exception("Hotel not found.");

            return _mapper.Map<HotelDto>(hotelEntity);
        }
    }

}
