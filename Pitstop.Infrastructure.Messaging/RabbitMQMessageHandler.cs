using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pitstop.Infrastructure.Messaging
{
    public class RabbitMQMessageHandler : IMessageHandler
    {
        private readonly ILogger<RabbitMQMessageHandler> _logger;
        private readonly InfrastructureMessagingOptions _options;

        private IConnection _connection;
        private IModel _model;
        private AsyncEventingBasicConsumer _consumer;
        private string _consumerTag;
        private IMessageHandlerCallback _callback;

        public RabbitMQMessageHandler(ILogger<RabbitMQMessageHandler> logger,
            IOptions<InfrastructureMessagingOptions> options
            )
        {
            _logger = logger;
            _options = options.Value;
        }

        public void Start(IMessageHandlerCallback callback)
        {
            _callback = callback;

            Policy
                .Handle<Exception>()
                .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { _logger.LogError("Error connecting to RabbitMQ. Retrying in 5 sec."); })
                .Execute(() =>
                {
                    var factory = new ConnectionFactory() { HostName = _options.Host, UserName = _options.Username,
                        Password = _options.Password, DispatchConsumersAsync = true };
                    _connection = factory.CreateConnection();
                    _model = _connection.CreateModel();
                    _model.ExchangeDeclare(_options.Exchange, "fanout", durable: true, autoDelete: false);
                    _model.QueueDeclare(_options.QueueName, durable: true, autoDelete: false, exclusive: false);
                    _model.QueueBind(_options.QueueName, _options.Exchange, _options.RoutingKey);
                    _consumer = new AsyncEventingBasicConsumer(_model);
                    _consumer.Received += Consumer_Received;
                    _consumerTag = _model.BasicConsume(_options.QueueName, false, _consumer);
                });
        }

        public void Stop()
        {
            _model.BasicCancel(_consumerTag);
            _model.Close(200, "Goodbye");
            _connection.Close();
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            if (await HandleEvent(ea))
            {
                _model.BasicAck(ea.DeliveryTag, false);
            }
        }

        private Task<bool> HandleEvent(BasicDeliverEventArgs ea)
        {
            // determine messagetype
            string messageType = Encoding.UTF8.GetString((byte[])ea.BasicProperties.Headers["MessageType"]);

            // get body
            string body = Encoding.UTF8.GetString(ea.Body);

            // call callback to handle the message
            return _callback.HandleMessageAsync(messageType, body);
        }
    }
}
