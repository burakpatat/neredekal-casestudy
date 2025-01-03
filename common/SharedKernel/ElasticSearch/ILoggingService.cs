
namespace SharedKernel.ElasticSearch
{
    public interface ILoggingService
    {
        void LogInformation(string message, object properties = null);
        void LogWarning(string message, object properties = null);
        void LogError(string message, Exception ex = null, object properties = null);
        void LogDebug(string message, object properties = null);
    }
}
