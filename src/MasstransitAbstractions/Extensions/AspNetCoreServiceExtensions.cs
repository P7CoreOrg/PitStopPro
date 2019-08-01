using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MassTransitAbstractions.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddMassTransitOptions(this IServiceCollection services, 
                                                    Action<MassTransitOptions> configureOptions)
        {
            services.Configure(configureOptions);
        }
        public static void AddMassTransitOptions(this IServiceCollection services,
                                                   IConfiguration config)
        {
            services.Configure<MassTransitOptions>(config);
        }
    }
}
