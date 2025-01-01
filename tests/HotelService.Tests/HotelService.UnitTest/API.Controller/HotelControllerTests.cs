

using HotelService.API.Controllers;
using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;
using HotelService.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedKernel.Enums;

namespace HotelService.UnitTest.API.Controller
{
    public class HotelControllerTests
    {
        private readonly Mock<IHotelService> _hotelServiceMock;
        private readonly HotelController _controller;

        public HotelControllerTests()
        {
            _hotelServiceMock = new Mock<IHotelService>();
            _controller = new HotelController(_hotelServiceMock.Object);
        }

        [Fact]
        public async Task CreateHotel_Should_Return_CreatedAtActionResult()
        {
            // Arrange
            var command = new CreateHotelCommand { Name = "Neredekal Test Hotel" };
            var createdHotelDto = new HotelDto { Id = Guid.NewGuid(), Name = command.Name };

            _hotelServiceMock.Setup(s => s.CreateHotelAsync(command)).ReturnsAsync(createdHotelDto);

            // Act
            var result = await _controller.CreateHotel(command);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(HotelController.GetHotelDetailById), createdAtActionResult.ActionName);
            Assert.Equal(createdHotelDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task DeleteHotel_Should_Return_NoContent_When_Successful()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            _hotelServiceMock.Setup(s => s.DeleteHotelAsync(hotelId)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteHotel(hotelId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteHotel_Should_Return_NotFound_When_Failed()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            _hotelServiceMock.Setup(s => s.DeleteHotelAsync(hotelId)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteHotel(hotelId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task AddContactInfo_Should_Return_OkObjectResult()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var contactInfoDto = new HotelContactInfoDto { Type = HotelContactInfoType.PhoneNumber, Value = "+90 123 456 7890" };

            _hotelServiceMock.Setup(s => s.AddHotelContactInfoAsync(hotelId, contactInfoDto)).ReturnsAsync(contactInfoDto);

            // Act
            var result = await _controller.AddContactInfo(hotelId, contactInfoDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(contactInfoDto, okResult.Value);
        }

        [Fact]
        public async Task RemoveContactInfo_Should_Return_OkObjectResult_When_Successful()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var contactInfoType = (int)HotelContactInfoType.Location;

            _hotelServiceMock.Setup(s => s.RemoveHotelContactInfoAsync(hotelId, contactInfoType)).ReturnsAsync(true);

            // Act
            var result = await _controller.RemoveContactInfo(hotelId, contactInfoType);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task RemoveContactInfo_Should_Return_NotFound_When_Failed()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var contactInfoType = (int)HotelContactInfoType.PhoneNumber;

            _hotelServiceMock.Setup(s => s.RemoveHotelContactInfoAsync(hotelId, contactInfoType)).ReturnsAsync(false);

            // Act
            var result = await _controller.RemoveContactInfo(hotelId, contactInfoType);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetHotelDetailById_Should_Return_OkObjectResult_When_Found()
        {
            // Arrange
            var hotelId = Guid.NewGuid();
            var hotelDto = new HotelDto { Id = hotelId, Name = "Neredekal Test Hotel" };

            _hotelServiceMock.Setup(s => s.GetHotelDetailByIdAsync(hotelId)).ReturnsAsync(hotelDto);

            // Act
            var result = await _controller.GetHotelDetailById(hotelId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(hotelDto, okResult.Value);
        }

        [Fact]
        public async Task GetHotelDetailById_Should_Return_NotFound_When_Not_Found()
        {
            // Arrange
            var hotelId = Guid.NewGuid();

            _hotelServiceMock.Setup(s => s.GetHotelDetailByIdAsync(hotelId)).ReturnsAsync((HotelDto)null);

            // Act
            var result = await _controller.GetHotelDetailById(hotelId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetHotel_Should_Return_OkObjectResult_When_Hotels_Found()
        {
            // Arrange
            var hotelList = new List<HotelDto>
        {
            new HotelDto { Id = Guid.NewGuid(), Name = "Hotel 1" },
            new HotelDto { Id = Guid.NewGuid(), Name = "Hotel 2" }
        };

            _hotelServiceMock.Setup(s => s.GetHotelDetailsAsync()).ReturnsAsync(hotelList);

            // Act
            var result = await _controller.GetHotel();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(hotelList, okResult.Value);
        }

        [Fact]
        public async Task GetHotel_Should_Return_NotFound_When_No_Hotels_Found()
        {
            // Arrange
            _hotelServiceMock.Setup(s => s.GetHotelDetailsAsync()).ReturnsAsync(new List<HotelDto>());

            // Act
            var result = await _controller.GetHotel();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No hotels found.", notFoundResult.Value);
        }
    }
}
