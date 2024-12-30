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
    public class AddHotelContactInfoCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddHotelContactInfoCommandHandler _handler;

        public AddHotelContactInfoCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new AddHotelContactInfoCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_Should_AddContactInfo_And_ReturnDto()
        {
            // Arrange
            var command = new AddHotelContactInfoCommand
            {
                HotelId = Guid.NewGuid(),
                Type = HotelContactInfoType.PhoneNumber,
                Value = "+90 123 456 7890"
            };

            var hotelEntity = new Hotel
            {
                Id = command.HotelId
            };

            var contactInfoEntity = new HotelContactInfo
            {
                Id = Guid.NewGuid(),
                HotelId = command.HotelId,
                Type = command.Type,
                Value = command.Value
            };

            var contactInfoDto = new HotelContactInfoDto
            {
                Id = contactInfoEntity.Id,
                HotelId = command.HotelId,
                Type = command.Type,
                Value = command.Value
            };

            var hotelRepositoryMock = new Mock<IRepository<Hotel>>();
            var contactInfoRepositoryMock = new Mock<IRepository<HotelContactInfo>>();

            hotelRepositoryMock.Setup(r => r.GetByIdAsync(command.HotelId)).ReturnsAsync(hotelEntity);
            contactInfoRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<HotelContactInfo>())).Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.GetRepository<Hotel>()).Returns(hotelRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.GetRepository<HotelContactInfo>()).Returns(contactInfoRepositoryMock.Object);
            _mapperMock.Setup(m => m.Map<HotelContactInfoDto>(It.IsAny<HotelContactInfo>())).Returns(contactInfoDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(contactInfoDto.Value, result.Value);

            hotelRepositoryMock.Verify(r => r.GetByIdAsync(command.HotelId), Times.Once);
            contactInfoRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<HotelContactInfo>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<HotelContactInfoDto>(It.IsAny<HotelContactInfo>()), Times.Once);
        }
    }

}
