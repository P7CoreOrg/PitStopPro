using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pitstop.Infrastructure.Messaging.Extensions
{
   
    public static class AspDotNetCoreExtensions
    {

        public static void AddInfrastructureMessaging(this IServiceCollection services,
            Action<InfrastructureMessagingOptions> setupAction)
        {
            services.Configure(setupAction);
            services.AddTransient<IMessagePublisher, RabbitMQMessagePublisher>();
            services.AddTransient<IMessageHandler, RabbitMQMessageHandler>();
        }
    }
}