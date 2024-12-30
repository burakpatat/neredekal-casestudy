
using HotelService.Application.Mediator.Commands;
using HotelService.Domain.Entities;
using HotelService.Infrastructure.Repository;
using HotelService.Infrastructure.UnitOfWork;
using Moq;

namespace HotelService.UnitTest.Application.CQRS
{
    public class DeleteHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly DeleteHotelCommandHandler _handler;

        public DeleteHotelCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new DeleteHotelCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteHotel_And_ReturnTrue()
        {
            // Arrange
            var command = new DeleteHotelCommand { HotelId = Guid.NewGuid() };
            var repositoryMock = new Mock<IRepository<Hotel>>();
            repositoryMock.Setup(r => r.GetByIdAsync(command.HotelId)).ReturnsAsync(new Hotel { Id = command.HotelId });
            repositoryMock.Setup(r => r.RemoveAsync(It.IsAny<Hotel>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.GetRepository<Hotel>()).Returns(repositoryMock.Object);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            repositoryMock.Verify(r => r.GetByIdAsync(command.HotelId), Times.Once);
            repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Hotel>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ReturnFalse_When_HotelNotFound()
        {
            // Arrange
            var command = new DeleteHotelCommand { HotelId = Guid.NewGuid() };
            var repositoryMock = new Mock<IRepository<Hotel>>();
            repositoryMock.Setup(r => r.GetByIdAsync(command.HotelId)).ReturnsAsync((Hotel)null);
            _unitOfWorkMock.Setup(u => u.GetRepository<Hotel>()).Returns(repositoryMock.Object);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            repositoryMock.Verify(r => r.GetByIdAsync(command.HotelId), Times.Once);
            repositoryMock.Verify(r => r.RemoveAsync(It.IsAny<Hotel>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }

}
