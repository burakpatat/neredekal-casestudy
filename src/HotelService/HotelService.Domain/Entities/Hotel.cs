
namespace HotelService.Domain.Entities
{
    public class Hotel : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<HotelRepresentative> Representatives { get; set; }
        public ICollection<HotelContactInfo> ContactInfos { get; set; }
    }
}
