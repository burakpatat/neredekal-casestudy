using ReportService.Domain.Entities;
using SharedKernel.Enums;

namespace ReportService.Application.Services
{
    public interface IReportService
    {
        Task<List<Report>> GetAllReportsAsync();
        Task<Report> GetReportByIdAsync(Guid reportId);
        Task CreateReportAsync(Report report);
        Task UpdateReportStatusAsync(Guid reportId, ReportStatus status);
    }
}
