
using AutoMapper;
using HotelService.Application.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;
using System.Reflection;

namespace HotelService.Application
{
    public static class ServiceRegistiration
    {
        public static void AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cf => cf.RegisterServicesFromAssembly(typeof(ServiceRegistiration).Assembly));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            var config = new MapperConfiguration(conf =>
            {
                conf.AddProfile<HotelMapping>();
            });

            services.AddScoped(s => config.CreateMapper());

            services.AddSharedKernel(configuration);
        }
    }
}
