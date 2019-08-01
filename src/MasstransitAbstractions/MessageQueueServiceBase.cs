using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace MassTransitAbstractions
{
    public abstract class MessageQueueServiceBase : BackgroundService
    {
        protected abstract void OnAddReceiveEndpoint(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host);

        protected MassTransitOptions _options;
        protected IBusControl _bus;

        public MessageQueueServiceBase(IOptions<MassTransitOptions> options)
        {
            _options = options.Value;
            switch (_options.TransportType)
            {
                case MassTransitOptions.Transport.RabbitMQ:
                    _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri("rabbitmq://localhost/"), h =>
                        {
                            h.Username(_options.Username);
                            h.Password(_options.Password);
                        });
                        OnAddReceiveEndpoint(cfg, host);
                    });
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"MasstransitOptions.Transport:{_options.TransportType} is not supported");
            }

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _bus.StartAsync();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _bus.StopAsync());
        }
    }
}
