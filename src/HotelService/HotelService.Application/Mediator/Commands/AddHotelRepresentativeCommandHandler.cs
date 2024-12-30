using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class AddHotelRepresentativeCommandHandler : IRequestHandler<AddHotelRepresentativeCommand, HotelRepresentativeDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddHotelRepresentativeCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HotelRepresentativeDto> Handle(AddHotelRepresentativeCommand request, CancellationToken cancellationToken)
        {
            var representativeRepository = _unitOfWork.GetRepository<HotelRepresentative>();
            var hotelRepository = _unitOfWork.GetRepository<Hotel>();

            var hotelEntity = await hotelRepository.GetByIdAsync(request.HotelId);
            if (hotelEntity == null)
                throw new Exception("Hotel not found.");

            var representative = new HotelRepresentative
            {
                FirstName = request.Name,
                LastName = request.SurName,
                HotelId = hotelEntity.Id
            };

            await representativeRepository.CreateAsync(representative);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<HotelRepresentativeDto>(representative);
        }
    }
}
