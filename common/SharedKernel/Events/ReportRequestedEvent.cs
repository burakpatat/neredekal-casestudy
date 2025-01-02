

using SharedKernel.Enums;

namespace SharedKernel.Events
{
    public class ReportRequestedEvent
    {
        public Guid ReportId { get; set; }
        public DateTime RequestedAt { get; set; }
        //public List<LocationReportData> Locations { get; set; }
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
        public ReportStatus ReportStatus { get; set; } = ReportStatus.Preparing;
    }

    public class LocationReportData
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
