
namespace ReportService.Domain.Entities
{
    public class LocationReport
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
        public Guid ReportId { get; set; }
        public Report Report { get; set; }
    }
}
