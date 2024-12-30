

using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateHotelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HotelDto> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelRepository = _unitOfWork.GetRepository<Hotel>();

            var hotelEntity = _mapper.Map<Hotel>(request);

            await hotelRepository.CreateAsync(hotelEntity);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<HotelDto>(hotelEntity);
        }
    }

}
