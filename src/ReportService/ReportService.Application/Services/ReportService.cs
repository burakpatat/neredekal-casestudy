using ReportService.Domain.Entities;
using SharedKernel.Enums;
using ReportService.Infrastructure.Repository;

namespace ReportService.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<Report> _reportRepository;

        public ReportService(IRepository<Report> reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<List<Report>> GetAllReportsAsync()
        {
            return await _reportRepository.GetAllAsync();
        }

        public async Task<Report> GetReportByIdAsync(Guid reportId)
        {
            return await _reportRepository.GetByIdAsync(reportId);
        }

        public async Task CreateReportAsync(Report report)
        {
            await _reportRepository.CreateAsync(report);
        }

        public async Task UpdateReportStatusAsync(Guid reportId, ReportStatus status)
        {
            var report = await _reportRepository.GetByIdAsync(reportId);
            if (report != null)
            {
                report.Status = status;
                await _reportRepository.UpdateAsync(reportId, report);
            }
        }
    }
}
