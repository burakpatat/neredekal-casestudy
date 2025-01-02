

namespace HotelService.Infrastructure.Repository
{
    public interface IHotelStatisticsRepository
    {
        Task<List<LocationStatistics>> GetHotelsGroupedByLocationAsync();
    }

    public class LocationStatistics
    {
        public string Location { get; set; }
        public int HotelCount { get; set; }
        public int PhoneCount { get; set; }
    }
}
