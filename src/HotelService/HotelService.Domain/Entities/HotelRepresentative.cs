
namespace HotelService.Domain.Entities
{
    public class HotelRepresentative : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
    }
}
