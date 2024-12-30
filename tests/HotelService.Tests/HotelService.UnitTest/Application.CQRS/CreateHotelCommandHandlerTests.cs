
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
        public async Task Handle_Should_CreateHotel_And_ReturnHotelDto()
        {
            // Arrange
            var command = new CreateHotelCommand
            {
                Name = "NeredeKal Hotel",
                Representatives = new List<HotelRepresentativeDto>
            {
                new HotelRepresentativeDto { FullName = "Burak Patat" }
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
                Name = "NeredeKal Hotel"
            };

            var hotelDto = new HotelDto
            {
                Id = hotelEntity.Id,
                Name = "NeredeKal Hotel"
            };

            var repositoryMock = new Mock<IRepository<Hotel>>();
            repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Hotel>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.GetRepository<Hotel>()).Returns(repositoryMock.Object);
            _mapperMock.Setup(m => m.Map<Hotel>(command)).Returns(hotelEntity);
            _mapperMock.Setup(m => m.Map<HotelDto>(hotelEntity)).Returns(hotelDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hotelDto.Name, result.Name);

            repositoryMock.Verify(r => r.CreateAsync(It.IsAny<Hotel>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<Hotel>(command), Times.Once);
            _mapperMock.Verify(m => m.Map<HotelDto>(hotelEntity), Times.Once);
        }
    }


}
