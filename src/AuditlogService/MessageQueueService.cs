using MassTransit;
using MassTransitAbstractions;
using Microsoft.Extensions.Options;
using MassTransit.RabbitMqTransport;
using MessageContracts;

namespace AuditlogService
{
    public class MessageQueueService : MessageQueueServiceBase
    {
        public MessageQueueService(IOptions<MassTransitOptions> options) : base(options)
        {

        }
        protected override void OnAddReceiveEndpoint(IRabbitMqBusFactoryConfigurator cfg, IRabbitMqHost host)
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
}
