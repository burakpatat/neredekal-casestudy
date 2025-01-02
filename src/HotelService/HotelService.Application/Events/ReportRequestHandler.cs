
using EventBus;
using HotelService.Infrastructure.Repository;
using HotelService.Infrastructure.UnitOfWork;
using SharedKernel.Events;

namespace HotelService.Application.Events
{
    public class ReportRequestHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;

        public ReportRequestHandler(IUnitOfWork unitOfWork, IEventBus eventBus)
        {
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
        }
        public async Task HandleReportRequest(Guid reportId)
        {
            var locations = await _unitOfWork.GetCustomRepository<HotelStatisticsRepository>().GetHotelsGroupedByLocationAsync();

            var locationReports = locations.Select(location => new LocationReportData
            {
                Location = location.Location,
                HotelCount = location.HotelCount,
                PhoneCount = location.PhoneCount
            }).ToList();

            var reportRequestedEvent = new ReportRequestedEvent
            {
                ReportId = reportId,
                RequestedAt = DateTime.UtcNow,
                Locations = locationReports
            };

            _eventBus.Publish(reportRequestedEvent);
        }
    }
}
