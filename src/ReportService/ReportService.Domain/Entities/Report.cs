
namespace ReportService.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Preparing;
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneNumberCount { get; set; }
    }
}
