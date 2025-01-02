using MongoDB.Driver;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Persistence;
using SharedKernel.Enums;
using SharedKernel.Events;

namespace ReportService.Application.Event
{
    public class ReportRequestedEventHandler
    {
        private readonly MongoDbContext _dbContext;

        public ReportRequestedEventHandler(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(ReportRequestedEvent reportEvent)
        {
            var report = new Report
            {
                Id = reportEvent.ReportId,
                RequestedDate = reportEvent.RequestedAt,
                Status = ReportStatus.Preparing,
                LocationReports = reportEvent.Locations.Select(location => new LocationReport
                {
                    Id = Guid.NewGuid(),
                    Location = location.Location,
                    HotelCount = location.HotelCount,
                    PhoneCount = location.PhoneCount
                }).ToList()
            };

            await _dbContext.Reports.InsertOneAsync(report);

            var filter = Builders<Report>.Filter.Eq(r => r.Id, report.Id);
            var update = Builders<Report>.Update.Set(r => r.Status, ReportStatus.Completed);

            await _dbContext.Reports.UpdateOneAsync(filter, update);
        }
    }
}
