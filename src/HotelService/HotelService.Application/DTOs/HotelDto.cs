
namespace HotelService.Application.DTOs
{
    public class HotelDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<HotelRepresentativeDto> Representatives { get; set; }
        public ICollection<HotelContactInfoDto> ContactInfos { get; set; }
    }
}
