

namespace HotelService.Infrastructure.Repository
{
    public interface IHotelStatisticsRepository
    {
        Task<LocationStatistics> GetHotelsGroupedByLocationAsync(string location);
    }

    public class LocationStatistics
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
