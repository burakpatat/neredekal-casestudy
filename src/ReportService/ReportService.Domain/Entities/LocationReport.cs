
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReportService.Domain.Entities
{
    public class LocationReport
    {
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
