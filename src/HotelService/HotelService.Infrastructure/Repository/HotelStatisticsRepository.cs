using HotelService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelService.Infrastructure.Repository
{
    public class HotelStatisticsRepository : IHotelStatisticsRepository
    {
        private readonly HotelDbContext _dbContext;

        public HotelStatisticsRepository(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LocationStatistics>> GetHotelsGroupedByLocationAsync()
        {
            return await _dbContext.HotelContactInfos
                .Where(ci => ci.Type == SharedKernel.Enums.HotelContactInfoType.Location)
                .GroupBy(ci => ci.Value)
                .Select(group => new LocationStatistics
                {
                    Location = group.Key, // Lokasyon bilgisi
                    HotelCount = group.Select(ci => ci.HotelId).Distinct().Count(), // Lokasyonda bulunan otel sayısı
                    PhoneCount = group.Sum(ci => ci.Hotel.ContactInfos.Count(c => c.Type == SharedKernel.Enums.HotelContactInfoType.PhoneNumber)) // Lokasyondaki telefon sayısı
                }).ToListAsync();
        }
    }
}
