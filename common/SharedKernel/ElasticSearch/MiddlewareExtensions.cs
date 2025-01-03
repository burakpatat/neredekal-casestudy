using Microsoft.AspNetCore.Builder;

namespace SharedKernel.ElasticSearch
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSharedLogging(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
