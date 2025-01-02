using MongoDB.Driver;
using ReportService.Domain.Entities;
using ReportService.Infrastructure.Repository;
using SharedKernel.Enums;
using SharedKernel.Events;

namespace ReportService.Application.Event
{
    public class ReportRequestedEventHandler
    {
        private readonly IRepository<Report> _reportRepository;

        public ReportRequestedEventHandler(IRepository<Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task Handle(ReportRequestedEvent reportEvent)
        {
            var report = new Report
            {
                Id = reportEvent.ReportId,
                RequestedDate = reportEvent.RequestedAt,
                Status = ReportStatus.Completed,
                Location = reportEvent.Location,
                HotelCount = reportEvent.HotelCount,
                PhoneCount = reportEvent.PhoneCount
            };

            // Rapor oluşturma
            await _reportRepository.CreateAsync(report);
        }
    }

}
