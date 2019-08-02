using MassTransit;
using MassTransitAbstractions;
using Microsoft.Extensions.Options;
using MassTransit.RabbitMqTransport;
using MessageContracts;
using System;
using MassTransitAbstractions.Extensions;

namespace AuditlogService
{
     
    public class OrderServiceEndpointRegistration : IMyRabbitMQReceiveEndpointRegistration
    {
        public void AddReceiveEndpoint(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host)
        {
            cfg.ReceiveEndpoint(host, "order-service", e =>
            {
                e.Handler<SubmitOrder>(context => context.RespondAsync<OrderAccepted>(new
                {
                    context.Message.OrderId
                }));
            });
        }
    }
    public class MessageQueueService : MessageQueueServiceBase
    {
        private IMyRabbitMQContainer _myRabbitMQContainer;
        public MessageQueueService(IMyRabbitMQContainer myRabbitMQContainer) : base(myRabbitMQContainer)
        {
            _myRabbitMQContainer = myRabbitMQContainer;
        }
    }
}
