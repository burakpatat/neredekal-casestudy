
using SharedKernel.Enums;

namespace HotelService.Application.DTOs
{
    public class HotelContactInfoDto
    {
        public HotelContactInfoType Type { get; set; }
        public string Value { get; set; }
    }
}
