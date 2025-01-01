using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Application.Mediator.Queries
{
    public class GetHotelRepresentativesQueryHandler : IRequestHandler<GetHotelRepresentativesQuery, List<HotelRepresentativeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetHotelRepresentativesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<HotelRepresentativeDto>> Handle(GetHotelRepresentativesQuery request, CancellationToken cancellationToken)
        {
            var hotelRepository = _unitOfWork.GetRepository<Hotel>();

            var hotelEntity = await hotelRepository.Table
            .Include(h => h.Representatives).FirstOrDefaultAsync(h => h.Id == request.HotelId, cancellationToken);

            if (hotelEntity == null) throw new Exception("Hotel not found.");

            var representatives = hotelEntity.Representatives;
            return _mapper.Map<List<HotelRepresentativeDto>>(representatives);
        }
    }


}
