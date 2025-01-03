using Microsoft.AspNetCore.Http;

namespace SharedKernel.ElasticSearch
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggingService loggingService)
        {
            loggingService.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path}");

            await _next(context);

            loggingService.LogInformation($"Outgoing Response: {context.Response.StatusCode}");
        }
    }

}
