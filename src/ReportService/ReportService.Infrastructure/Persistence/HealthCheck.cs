using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace ReportService.Infrastructure.Persistence
{
    public class HealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public HealthCheck(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                // MongoDB Health Check
                var mongoClient = new MongoClient(_configuration["MongoDb:ConnectionString"]);
                var mongoDatabase = mongoClient.GetDatabase(_configuration["MongoDb:DatabaseName"]);

                var isMongoHealthy = await mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                if (isMongoHealthy == null)
                {
                    return HealthCheckResult.Unhealthy("MongoDB is not healthy.");
                }

                // RabbitMQ Health Check
                var rabbitMqHost = _configuration["RabbitMq:Host"];
                var rabbitMqUsername = _configuration["RabbitMq:Username"];
                var rabbitMqPassword = _configuration["RabbitMq:Password"];

                if (string.IsNullOrWhiteSpace(rabbitMqHost) || string.IsNullOrWhiteSpace(rabbitMqUsername) || string.IsNullOrWhiteSpace(rabbitMqPassword))
                {
                    return HealthCheckResult.Unhealthy("RabbitMQ configuration is missing or incomplete.");
                }

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(rabbitMqHost),
                    UserName = rabbitMqUsername,
                    Password = rabbitMqPassword
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare("healthcheck-queue", true, false, false);
                }

                return HealthCheckResult.Healthy("MongoDB and RabbitMQ are healthy.");
            }
            catch (MongoException mongoEx)
            {
                return HealthCheckResult.Unhealthy($"MongoDB is not healthy: {mongoEx.Message}");
            }
            catch (BrokerUnreachableException rabbitEx)
            {
                return HealthCheckResult.Unhealthy($"RabbitMQ is not reachable: {rabbitEx.Message}");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Service is not healthy: {ex.Message}");
            }
        }
    }
}
