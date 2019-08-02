using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace MassTransitAbstractions.Extensions
{
     
    public interface IRabbitMQReceiveEndpointRegistration
    {
        void AddReceiveEndpoint(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host);
    }
    public interface IBusControlContainer
    {
        IBusControl BusControl { get; set; }
    }
    public interface IRabbitMQContainer: IBusControlContainer
    {
        IRabbitMqHost RabbitMqHost { get; set; }

    }
    public abstract class BusControlContainerBase : IBusControlContainer
    {
        public IBusControl BusControl { get; set; }
        public IPublishEndpoint PublishEndpoint => BusControl;
        public ISendEndpointProvider SendEndpointProvider => BusControl;
        public IBus Bus => BusControl;
    }
    public abstract class RabbitMQContainerBase : BusControlContainerBase, IRabbitMQContainer
    {
        public IRabbitMqHost RabbitMqHost { get; set; }
    }
    public static class AspNetCoreServiceExtensions
    {
        public static void AddRabbitMqMassTransitOptions<TOptions>(
            this IServiceCollection services,Action<MassTransitOptions> configureOptions) 
            where TOptions : MassTransitOptions, new()
        {
            services.Configure<TOptions>(configureOptions);
        }
        public static void AddRabbitMqMassTransitOptions<TOptions>(
            this IServiceCollection services,IConfiguration config) 
            where TOptions : MassTransitOptions, new()
        {
            services.Configure<TOptions>(config);
        }


        public static void AddRabbitMQMassTransit<TOptions, TService, TImplementation, THandlerRegistrants>(this IServiceCollection services)
            where TOptions : MassTransitOptions, new()
            where TService : class, IRabbitMQContainer
            where TImplementation : class, TService, new()
            where THandlerRegistrants : class, IRabbitMQReceiveEndpointRegistration

        {
            services.AddSingleton<TService>((sp) => {

                var options = sp.GetRequiredService<IOptions<TOptions>>();
                var opts = options.Value;
                IBusControl bus = null;
                TImplementation container = null;
                switch (opts.TransportType)
                {
                    case MassTransitOptions.Transport.RabbitMQ:
                        IRabbitMqHost host = null;
                        bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                        {
                            var uriString = $"rabbitmq://{opts.Host}/";
                            host = cfg.Host(new Uri(uriString), h =>
                            {
                                h.Username(opts.Username);
                                h.Password(opts.Password);
                            });
                            var registrants = sp.GetRequiredService<IEnumerable<THandlerRegistrants>>();
                            if (registrants != null && registrants.Any())
                            {
                                foreach (var registrant in registrants)
                                {
                                    registrant.AddReceiveEndpoint(cfg, host);
                                }
                            }
                        });
                        container = new TImplementation { BusControl = bus, RabbitMqHost = host };
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"MasstransitOptions.Transport:{opts.TransportType} is not supported");
                }
//                bus.Start();
             
                return container;
            });
        }
    }
}
