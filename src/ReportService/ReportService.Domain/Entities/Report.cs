
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SharedKernel.Enums;

namespace ReportService.Domain.Entities
{
    public class Report
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public DateTime RequestedDate { get; set; }
        public ReportStatus Status { get; set; } = ReportStatus.Preparing;
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
