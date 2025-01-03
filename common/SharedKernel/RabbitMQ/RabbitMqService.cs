using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedKernel.ElasticSearch;
using System.Text;

namespace SharedKernel.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnectionFactory _connectionFactory;
        private SerilogLoggingService _logger;

        public RabbitMqService(IConfiguration config)
        {
            var rabbitMqHost = config["RabbitMq:Host"];
            _connectionFactory = new ConnectionFactory { Uri = new Uri(rabbitMqHost) };

            _logger = new SerilogLoggingService(config);
            _logger.LogInformation("RabbitMqService initialized", new { rabbitMqHost });
        }

        public async Task SendMessageAsync<T>(string queueName, T message, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete, arguments: null);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                _logger.LogInformation("Message sent to queue", new { queueName, message });
            }
            await Task.CompletedTask;
        }

        public Task ReceiveMessageAsync(string queueName, Func<string, Task> handler, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            var connection = _connectionFactory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete, arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                _logger.LogInformation("Message received from queue", new { queueName, message });

                try
                {
                    await handler(message);
                    _logger.LogInformation("Message processed successfully", new { queueName, message });
                }
                catch (Exception handlerEx)
                {
                    _logger.LogError("Error in message handler", handlerEx, new { queueName, message });
                    throw;
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            _logger.LogInformation("Consumer subscribed to queue", new { queueName });

            return Task.CompletedTask;
        }
    }
}
