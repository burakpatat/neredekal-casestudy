
using EventBus;
using Newtonsoft.Json;

namespace SharedKernel.RabbitMQ
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IRabbitMqService _rabbitMqService;

        public RabbitMqEventBus(IRabbitMqService rabbitMqService)
        {
            _rabbitMqService = rabbitMqService;
        }

        public void Publish<T>(T @event)
        {
            _rabbitMqService.SendMessageAsync("event_queue", @event);
        }

        public void Subscribe<T>(Func<T, Task> handler)
        {
            _rabbitMqService.ReceiveMessageAsync("event_queue", async (message) =>
            {
                var typedMessage = JsonConvert.DeserializeObject<T>(message);
                await handler(typedMessage);
            });
        }
    }
}
