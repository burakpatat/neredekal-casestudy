
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReportService.Infrastructure.Repository;

namespace ReportService.Infrastructure.Persistence
{
    public static class ServiceRegistiration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //healthcheck
            services.AddHealthChecks()
                    .AddCheck<HealthCheck>("MongoDB & RabbitMQ");

            // MongoDB
            services.AddSingleton<MongoDbContext>(sp =>
            {
                var mongoConfig = configuration.GetSection("MongoDb");
                var connectionString = mongoConfig["ConnectionString"];
                var databaseName = mongoConfig["DatabaseName"];

                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
                {
                    throw new InvalidOperationException("MongoDB connection string or database name is not configured properly.");
                }

                return new MongoDbContext(connectionString, databaseName);
            });

            // Generic Repository
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

        }
    }
}
