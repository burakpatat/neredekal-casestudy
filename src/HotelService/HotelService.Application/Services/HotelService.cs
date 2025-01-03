using EventBus;
using HotelService.Application.DTOs;
using HotelService.Application.Mediator.Commands;
using HotelService.Application.Mediator.Queries;
using HotelService.Infrastructure.Repository;
using HotelService.Infrastructure.UnitOfWork;
using MediatR;
using SharedKernel.ElasticSearch;
using SharedKernel.Events;

namespace HotelService.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEventBus _eventBus;
        private readonly ILoggingService _logger;

        public HotelService(IMediator mediator, IUnitOfWork unitOfWork, IEventBus eventBus, ILoggingService loggingService)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _eventBus = eventBus;
            _logger = loggingService;
        }

        public async Task<HotelDto> CreateHotelAsync(CreateHotelCommand command)
        {
            _logger.LogInformation("CreateHotel called", command);

            var hotelDto = await _mediator.Send(command);

            _logger.LogInformation("CreateHotel completed", hotelDto);
            return hotelDto;
        }

        public async Task<HotelRepresentativeDto> AddHotelRepresentativeAsync(Guid hotelId, HotelRepresentativeDto representative)
        {
            _logger.LogInformation("AddHotelRepresentative", new { hotelId, representative });

            var command = new AddHotelRepresentativeCommand
            {
                HotelId = hotelId,
                Name = representative.Name,
                SurName = representative.SurName
            };

            return await _mediator.Send(command);
        }

        public async Task<bool> DeleteHotelAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteHotelCommand { HotelId = id });
            return result;
        }

        public async Task<HotelContactInfoDto> AddHotelContactInfoAsync(Guid hotelId, HotelContactInfoDto contactInfo)
        {
            _logger.LogInformation("AddHotelContactInfo", new { hotelId, contactInfo });
            try
            {
                var command = new AddHotelContactInfoCommand
                {
                    HotelId = hotelId,
                    Type = contactInfo.Type,
                    Value = contactInfo.Value
                };
                var result = await _mediator.Send(command);
                _logger.LogInformation("AddHotelContactInfo", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in AddHotelContactInfo", ex, new { hotelId, contactInfo });
                throw;
            }
        }

        public async Task<bool> RemoveHotelContactInfoAsync(Guid hotelId, int contactInfoType)
        {
            var command = new RemoveHotelContactInfoCommand { HotelId = hotelId, Type = contactInfoType };
            return await _mediator.Send(command);
        }

        public async Task<IEnumerable<HotelRepresentativeDto>> GetHotelRepresentativesAsync(Guid hotelId)
        {
            var query = new GetHotelRepresentativesQuery { HotelId = hotelId };
            return await _mediator.Send(query);
        }

        public async Task<HotelDto> GetHotelDetailByIdAsync(Guid hotelId)
        {
            var query = new GetHotelDetailsQueryById { HotelId = hotelId };
            return await _mediator.Send(query);
        }

        public async Task<List<HotelDto>> GetHotelDetailsAsync()
        {
            var query = new GetHotelDetailsQuery();
            return await _mediator.Send(query);
        }

        public async Task<ReportRequestedEvent> StartLocationBasedReportAsync(Guid reportId, string location)
        {
            _logger.LogInformation("Create Report called", new { reportId, location });
            try
            {
                var locations = await _unitOfWork.GetCustomRepository<HotelStatisticsRepository>().GetHotelsGroupedByLocationAsync(location);

                var reportRequestedEvent = new ReportRequestedEvent
                {
                    ReportId = reportId,
                    RequestedAt = DateTime.UtcNow,
                    Location = locations.Location,
                    HotelCount = locations.HotelCount,
                    PhoneCount = locations.PhoneCount,
                    ReportStatus = SharedKernel.Enums.ReportStatus.Preparing
                };

                _eventBus.Publish(reportRequestedEvent);

                _logger.LogInformation("Report completed - Event Publish", reportRequestedEvent);
                return reportRequestedEvent;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Created Report", ex, new { reportId, location });
                throw;
            }
        }
    }


}
