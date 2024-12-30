

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
            var hotelEntity = new Hotel
            {
                Name = request.Name
            };

            var hotelRepository = _unitOfWork.GetRepository<Hotel>();
            await hotelRepository.CreateAsync(hotelEntity);
            await _unitOfWork.SaveChangesAsync();

            if (request.Representatives != null && request.Representatives.Any())
            {
                foreach (var representative in request.Representatives)
                {
                    var representativeEntity = new HotelRepresentative
                    {
                        FirstName = representative.Name,
                        LastName = representative.SurName,
                        HotelId = hotelEntity.Id
                    };

                    await _unitOfWork.GetRepository<HotelRepresentative>().CreateAsync(representativeEntity);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            if (request.ContactInfos != null && request.ContactInfos.Any())
            {
                foreach (var contactInfo in request.ContactInfos)
                {
                    var contactInfoEntity = new HotelContactInfo
                    {
                        Type = contactInfo.Type,
                        Value = contactInfo.Value,
                        HotelId = hotelEntity.Id
                    };

                    await _unitOfWork.GetRepository<HotelContactInfo>().CreateAsync(contactInfoEntity);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return _mapper.Map<HotelDto>(hotelEntity);
        }
    }

}
