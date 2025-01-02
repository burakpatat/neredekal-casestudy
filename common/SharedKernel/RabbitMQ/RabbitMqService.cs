using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SharedKernel.RabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly IConnectionFactory _connectionFactory;

        public RabbitMqService(IConfiguration config)
        {
            var rabbitMqHost = config["RabbitMqHost"];
            _connectionFactory = new ConnectionFactory { Uri = new Uri(rabbitMqHost) };
        }

        public async Task SendMessageAsync<T>(string queueName, T message, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete, arguments: null);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
            }
        }

        public async Task ReceiveMessageAsync(string queueName, Func<string, Task> handler, bool durable = true, bool exclusive = false, bool autoDelete = false)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: durable, exclusive: exclusive, autoDelete: autoDelete, arguments: null);
                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += async (model, e) =>
                {
                    var body = e.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    await handler(message);
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            }
        }
    }
}
