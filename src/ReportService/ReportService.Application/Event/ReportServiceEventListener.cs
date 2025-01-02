using EventBus;
using Microsoft.Extensions.Hosting;
using SharedKernel.Events;

namespace ReportService.Application.Event
{
    public class ReportServiceEventListener : BackgroundService
    {
        private readonly IEventBus _eventBus;
        private readonly ReportRequestedEventHandler _eventHandler;

        public ReportServiceEventListener(IEventBus eventBus, ReportRequestedEventHandler eventHandler)
        {
            _eventBus = eventBus;
            _eventHandler = eventHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // RabbitMQ kuyruğundan gelen ReportRequestedEvent'i dinliyoruz
            _eventBus.Subscribe<ReportRequestedEvent>(async (reportEvent) =>
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    await _eventHandler.Handle(reportEvent);
                }
            });
            await Task.CompletedTask;
        }
    }
}
