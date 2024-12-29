
namespace HotelService.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<HotelRepresentative> Representatives { get; set; }
        public ICollection<HotelContactInfo> ContactInfos { get; set; }
    }
}
