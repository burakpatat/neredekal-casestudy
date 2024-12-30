using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;

namespace HotelService.Application.Mediator.Commands
{
    public class AddHotelContactInfoCommandHandler : IRequestHandler<AddHotelContactInfoCommand, HotelContactInfoDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddHotelContactInfoCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<HotelContactInfoDto> Handle(AddHotelContactInfoCommand request, CancellationToken cancellationToken)
        {
            var hotelRepository = _unitOfWork.GetRepository<Hotel>();
            var contactInfoRepository = _unitOfWork.GetRepository<HotelContactInfo>();

            var hotelEntity = await hotelRepository.GetByIdAsync(request.HotelId);
            if (hotelEntity == null) throw new Exception("Hotel not found.");

            var contactInfo = new HotelContactInfo
            {
                Type = request.Type,
                Value = request.Value,
                HotelId = hotelEntity.Id
            };

            await contactInfoRepository.CreateAsync(contactInfo);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<HotelContactInfoDto>(contactInfo);
        }
    }

}
