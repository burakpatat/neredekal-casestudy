
namespace SharedKernel.RabbitMQ
{
    public interface IRabbitMqService
    {
        Task SendMessageAsync<T>(string queueName, T message, bool durable = true, bool exclusive = false, bool autoDelete = false);
        Task ReceiveMessageAsync(string queueName, Func<string, Task> handler, bool durable = true, bool exclusive = false, bool autoDelete = false);
    }
}
