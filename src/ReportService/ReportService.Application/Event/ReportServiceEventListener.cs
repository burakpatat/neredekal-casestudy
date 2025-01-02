using EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.Events;

namespace ReportService.Application.Event
{
    public class ReportServiceEventListener : BackgroundService
    {
        private readonly IEventBus _eventBus;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ReportServiceEventListener(IEventBus eventBus, IServiceScopeFactory serviceScopeFactory)
        {
            _eventBus = eventBus;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // RabbitMQ kuyruğundan gelen ReportRequestedEvent'i dinliyoruz
            _eventBus.Subscribe<ReportRequestedEvent>(async (reportEvent) =>
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var eventHandler = scope.ServiceProvider.GetRequiredService<ReportRequestedEventHandler>();
                    await eventHandler.Handle(reportEvent);
                }
            });

            await Task.CompletedTask;
        }
    }

}
