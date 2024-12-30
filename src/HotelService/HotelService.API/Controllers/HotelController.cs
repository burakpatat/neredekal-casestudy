using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;
using HotelService.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // Yeni bir otel oluşturmak içi
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelCommand command)
        {
            var result = await _hotelService.CreateHotelAsync(command);
            return CreatedAtAction(nameof(GetHotelDetails), new { hotelId = result.Id }, result);
        }

        // Oteli silmek için
        [HttpDelete("{hotelId:guid}")]
        public async Task<IActionResult> DeleteHotel(Guid hotelId)
        {
            var result = await _hotelService.DeleteHotelAsync(hotelId);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Otel iletişim bilgisi eklemek için
        [HttpPost("{hotelId:guid}/contact-info")]
        public async Task<IActionResult> AddContactInfo(Guid hotelId, [FromBody] HotelContactInfoDto contactInfo)
        {
            var result = await _hotelService.AddHotelContactInfoAsync(hotelId, contactInfo);
            return Ok(result);
        }

        // Otel iletişim bilgisini silmek için
        [HttpDelete("contact-info/{contactInfoId:guid}")]
        public async Task<IActionResult> RemoveContactInfo(Guid contactInfoId)
        {
            var result = await _hotelService.RemoveHotelContactInfoAsync(contactInfoId);
            if (!result)
                return NotFound();
            return NoContent();
        }

        // Otel yetkililerini eklemek için
        [HttpPost("{hotelId:guid}/representatives")]
        public async Task<IActionResult> AddRepresentatives(Guid hotelId, [FromBody] IEnumerable<HotelRepresentativeDto> representatives)
        {
            foreach (var representative in representatives)
            {
                var result = await _hotelService.AddHotelRepresentativeAsync(hotelId, representative);
                if (result == null)
                    return BadRequest("Yetkili eklenemedi.");
            }
            return Ok();
        }

        // Otel yetkililerini almak için
        [HttpGet("{hotelId:guid}/representatives")]
        public async Task<IActionResult> GetRepresentatives(Guid hotelId)
        {
            var result = await _hotelService.GetHotelRepresentativesAsync(hotelId);
            return Ok(result);
        }

        // Otelin detaylarını almak için
        [HttpGet("{hotelId:guid}")]
        public async Task<IActionResult> GetHotelDetails(Guid hotelId)
        {
            var result = await _hotelService.GetHotelDetailsAsync(hotelId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
