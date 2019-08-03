using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransitAbstractions.Extensions;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace MassTransitAbstractions
{
    public abstract class MessageQueueServiceBase : BackgroundService
    {
        private IBusControlContainer _busControlContainer;

        public MessageQueueServiceBase(IBusControlContainer busControlContainer)
        {
            _busControlContainer = busControlContainer;

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busControlContainer.BusControl.StartAsync();
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.WhenAll(base.StopAsync(cancellationToken), _busControlContainer.BusControl.StopAsync());
        }
    }
}
