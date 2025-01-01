
using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.Repository;
using HotelService.Infrastructure.UnitOfWork;
using Moq;
using SharedKernel.Enums;

namespace HotelService.UnitTest.Application.CQRS
{
    public class CreateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateHotelCommandHandler _handler;

        public CreateHotelCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateHotelCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_CreateHotel_WithRepresentativesAndContactInfos_AndReturnHotelDto()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                Name = "NeredeKal Hotel",
                Representatives = new List<HotelRepresentativeDto>
                {
                    new HotelRepresentativeDto { Name = "Burak", SurName = "Patat" }
                },
                ContactInfos = new List<HotelContactInfoDto>
                {
                    new HotelContactInfoDto { Type = HotelContactInfoType.Email, Value = "test@neredekal.com" },
                    new HotelContactInfoDto { Type = HotelContactInfoType.Location, Value = "Istanbul" }
                }
            };

            var hotelEntity = new Hotel
            {
                Id = Guid.NewGuid(),
                Name = command.Name
            };

            var hotelDto = new HotelDto
            {
                Id = hotelEntity.Id,
                Name = hotelEntity.Name
            };

            var representativeEntity = new HotelRepresentative
            {
                HotelId = hotelEntity.Id,
                FirstName = command.Representatives.First().Name,
                LastName = command.Representatives.First().SurName
            };

            var contactInfoEntities = command.ContactInfos.Select(c => new HotelContactInfo
            {
                HotelId = hotelEntity.Id,
                Type = c.Type,
                Value = c.Value
            }).ToList();

            var hotelRepositoryMock = new Mock<IRepository<Hotel>>();
            var representativeRepositoryMock = new Mock<IRepository<HotelRepresentative>>();
            var contactInfoRepositoryMock = new Mock<IRepository<HotelContactInfo>>();

            hotelRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Hotel>())).Returns(Task.CompletedTask);
            hotelRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(hotelEntity);

            representativeRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<HotelRepresentative>())).Returns(Task.CompletedTask);
            contactInfoRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<HotelContactInfo>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.GetRepository<Hotel>()).Returns(hotelRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.GetRepository<HotelRepresentative>()).Returns(representativeRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.GetRepository<HotelContactInfo>()).Returns(contactInfoRepositoryMock.Object);

            _mapperMock.Setup(m => m.Map<HotelDto>(It.IsAny<Hotel>())).Returns(hotelDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hotelDto.Id, result.Id);
            Assert.Equal(hotelDto.Name, result.Name);

            hotelRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Hotel>()), Times.Once);
            representativeRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<HotelRepresentative>()), Times.Once);
            contactInfoRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<HotelContactInfo>()), Times.Exactly(command.ContactInfos.Count));

            _mapperMock.Verify(m => m.Map<HotelDto>(It.IsAny<Hotel>()), Times.Once);
        }

    }
}
