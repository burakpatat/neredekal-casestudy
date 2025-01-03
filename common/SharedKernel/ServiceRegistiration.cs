using EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using SharedKernel.ElasticSearch;
using SharedKernel.RabbitMQ;

namespace SharedKernel
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services, IConfiguration config, IHostBuilder builder)
        {
            services.AddSingleton<IRabbitMqService>(provider => new RabbitMqService(config));
            services.AddSingleton<IEventBus, RabbitMqEventBus>(provider =>
            new RabbitMqEventBus(provider.GetRequiredService<IRabbitMqService>(), config));

            // Serilog
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(config["Serilog:WriteTo:1:Args:node"]))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = config["Serilog:WriteTo:1:Args:indexFormat"],
                    BufferBaseFilename = "./logs/buffer",
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.ThrowException
                })
                .CreateLogger();

            services.AddSingleton<ILogger>(logger);
            services.AddSingleton<ILoggingService, SerilogLoggingService>();

            return services;
        }
    }
}
