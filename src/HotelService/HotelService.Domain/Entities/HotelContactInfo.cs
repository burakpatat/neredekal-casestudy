
namespace HotelService.Domain.Entities
{
    public class HotelContactInfo : BaseEntity
    {
        public HotelContactInfoType Type { get; set; }
        public string Value { get; set; }

        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
