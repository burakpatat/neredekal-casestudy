
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;

namespace SharedKernel.ElasticSearch
{
    public class SerilogLoggingService : ILoggingService
    {
        private readonly ILogger _logger;

        public SerilogLoggingService(IConfiguration configuration)
        {
            try
            {
                string elasticsearchUri = configuration["Serilog:WriteTo:1:Args:node"];
                string indexFormat = configuration["Serilog:WriteTo:1:Args:indexFormat"];

                _logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .WriteTo.Console(new JsonFormatter())
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUri))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = indexFormat,
                        BufferBaseFilename = "./logs/buffer",
                        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback | EmitEventFailureHandling.ThrowException
                    })
                    .CreateLogger();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Serilog yapılandırması sırasında bir hata oluştu.", ex);
            }
        }

        public void LogInformation(string message, object properties = null)
        {
            if (properties == null)
                _logger.Information("{@Message}", message);
            else
                _logger.Information("{@Message} {@Properties}", message, properties);
        }

        public void LogWarning(string message, object properties = null)
        {
            if (properties == null)
                _logger.Warning("{@Message}", message);
            else
                _logger.Warning("{@Message} {@Properties}", message, properties);
        }

        public void LogError(string message, Exception ex = null, object properties = null)
        {
            if (ex == null && properties == null)
                _logger.Error("{@Message}", message);
            else if (properties == null)
                _logger.Error(ex, "{@Message}", message);
            else
                _logger.Error(ex, "{@Message} {@Properties}", message, properties);
        }

        public void LogDebug(string message, object properties = null)
        {
            if (properties == null)
                _logger.Debug("{@Message}", message);
            else
                _logger.Debug("{@Message} {@Properties}", message, properties);
        }
    }

}
