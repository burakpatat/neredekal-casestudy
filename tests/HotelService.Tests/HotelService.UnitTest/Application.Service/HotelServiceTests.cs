using EventBus;
using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;
using HotelService.Application.Mediator.Queries;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;
using Moq;

namespace HotelService.UnitTest.Application.Service
{
    public class HotelServiceTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IUnitOfWork> _unitofworkMock;
        private readonly Mock<IEventBus> _eventbusMock;
        private readonly HotelService.Application.Services.HotelService _hotelService;

        public HotelServiceTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _unitofworkMock = new Mock<IUnitOfWork>();
            _eventbusMock = new Mock<IEventBus>();
            _hotelService = new HotelService.Application.Services.HotelService(_mediatorMock.Object, _unitofworkMock.Object, _eventbusMock.Object);
        }

        [Fact]
        public async Task CreateHotelAsync_ShouldReturnHotelDto()
        {
            var command = new CreateHotelCommand { Name = "Test Hotel" };
            var expectedHotel = new HotelDto { Id = Guid.NewGuid(), Name = "Test Hotel" };

            _mediatorMock
                .Setup(m => m.Send(command, default))
                .ReturnsAsync(expectedHotel);

            var result = await _hotelService.CreateHotelAsync(command);

            Assert.NotNull(result);
            Assert.Equal(expectedHotel.Name, result.Name);
            _mediatorMock.Verify(m => m.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task AddHotelRepresentativeAsync_ShouldReturnRepresentativeDto()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var representative = new HotelRepresentativeDto { Name = "Burak", SurName = "Patat" };
            var command = new AddHotelRepresentativeCommand
            {
                HotelId = hotelId,
                Name = representative.Name,
                SurName = representative.SurName
            };

            _mediatorMock
                .Setup(m => m.Send(It.Is<AddHotelRepresentativeCommand>(c =>
                    c.HotelId == hotelId && c.Name == representative.Name && c.SurName == representative.SurName), default))
                .ReturnsAsync(representative);

            // Act
            var result = await _hotelService.AddHotelRepresentativeAsync(hotelId, representative);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(representative.Name, result.Name);
            Assert.Equal(representative.SurName, result.SurName);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AddHotelRepresentativeCommand>(), default), Times.Once);
        }


        [Fact]
        public async Task DeleteHotelAsync_ShouldReturnTrue()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var command = new DeleteHotelCommand { HotelId = hotelId };

            _mediatorMock
                .Setup(m => m.Send(It.Is<DeleteHotelCommand>(c => c.HotelId == hotelId), default))
                .ReturnsAsync(true);

            // Act
            var result = await _hotelService.DeleteHotelAsync(hotelId);

            // Assert
            Assert.True(result);
            _mediatorMock.Verify(m => m.Send(It.IsAny<DeleteHotelCommand>(), default), Times.Once);
        }


        [Fact]
        public async Task GetHotelDetailsAsync_ShouldReturnListOfHotels()
        {
            // Arrange
            var expectedHotels = new List<HotelDto>
            {
                new HotelDto { Id = Guid.NewGuid(), Name = "Hotel A" },
                new HotelDto { Id = Guid.NewGuid(), Name = "Hotel B" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetHotelDetailsQuery>(), default))
                .ReturnsAsync(expectedHotels);

            // Act
            var result = await _hotelService.GetHotelDetailsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedHotels[0].Name, result[0].Name);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetHotelDetailsQuery>(), default), Times.Once);
        }


        [Fact]
        public async Task GetHotelDetailByIdAsync_ShouldReturnHotelDto()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var expectedHotel = new HotelDto { Id = hotelId, Name = "Hotel Test" };

            _mediatorMock
                .Setup(m => m.Send(It.Is<GetHotelDetailsQueryById>(q => q.HotelId == hotelId), default))
                .ReturnsAsync(expectedHotel);

            // Act
            var result = await _hotelService.GetHotelDetailByIdAsync(hotelId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedHotel.Id, result.Id);
            _mediatorMock.Verify(m => m.Send(It.IsAny<GetHotelDetailsQueryById>(), default), Times.Once);
        }

    }
}
