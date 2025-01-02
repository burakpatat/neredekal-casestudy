
using EventBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SharedKernel.RabbitMQ
{
    public class RabbitMqEventBus : IEventBus
    {
        private readonly IRabbitMqService _rabbitMqService;
        private readonly string? _queueName;

        public RabbitMqEventBus(IRabbitMqService rabbitMqService, IConfiguration configuration)
        {
            _rabbitMqService = rabbitMqService;
            _queueName = configuration["RabbitMq:Queue"];
        }

        public void Publish<T>(T @event)
        {
            Console.WriteLine($"Publishing event: {typeof(T).Name}");
            _rabbitMqService.SendMessageAsync(_queueName, @event);
        }

        public void Subscribe<T>(Func<T, Task> handler)
        {
            Console.WriteLine($"Subscribing to event: {typeof(T).Name}");
            _rabbitMqService.ReceiveMessageAsync(_queueName, async (message) =>
            {
                Console.WriteLine($"Received message: {message}");
                var typedMessage = JsonConvert.DeserializeObject<T>(message);
                if (typedMessage != null)
                {
                    await handler(typedMessage);
                }
            });
        }
    }
}
