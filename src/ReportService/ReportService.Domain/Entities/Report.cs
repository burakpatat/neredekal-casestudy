
using SharedKernel.Enums;

namespace ReportService.Domain.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Preparing;
        public ICollection<LocationReport> LocationReports { get; set; }
    }
}
