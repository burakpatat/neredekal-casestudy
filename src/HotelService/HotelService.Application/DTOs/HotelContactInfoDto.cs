
using SharedKernel.Enums;

namespace HotelService.Application.DTOs
{
    public class HotelContactInfoDto
    {
        public Guid Id { get; set; }
        public HotelContactInfoType Type { get; set; }
        public string Value { get; set; }
        public Guid HotelId { get; set; }
    }
}
