using EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.RabbitMQ;

namespace SharedKernel
{
    public static class ServiceRegistiration
    {
        public static IServiceCollection AddSharedKernel(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IRabbitMqService>(provider => new RabbitMqService(config));
            services.AddSingleton<IEventBus, RabbitMqEventBus>();

            return services;
        }
    }
}
