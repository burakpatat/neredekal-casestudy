using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Application.Mediator.Queries
{
    public class GetHotelQueryHandler : IRequestHandler<GetHotelDetailsQuery, List<HotelDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHotelQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<HotelDto>> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
        {
            var repository = _unitOfWork.GetRepository<Hotel>();

            var hotelEntity = await repository.Table
                .Include(h => h.Representatives)
                .Include(h => h.ContactInfos).ToListAsync();

            var mapList = _mapper.Map<List<HotelDto>>(hotelEntity);
            return mapList;
        }
    }
}
