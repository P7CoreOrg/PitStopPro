using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pitstop.Infrastructure.Messaging
{
    /// <summary>
    /// RabbitMQ implementation of the MessagePublisher.
    /// </summary>
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly ILogger<RabbitMQMessagePublisher> _logger;
        private readonly InfrastructureMessagingOptions _options;

        public RabbitMQMessagePublisher(
            ILogger<RabbitMQMessagePublisher> logger,
            IOptions<InfrastructureMessagingOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Publish a message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="message">The message to publish.</param>
        /// <param name="routingKey">The routingkey to use (RabbitMQ specific).</param>
        public Task PublishMessageAsync(string messageType, object message, string routingKey)
        {
            return Task.Run(() =>
                Policy
                    .Handle<Exception>()
                    .WaitAndRetry(9, r => TimeSpan.FromSeconds(5), (ex, ts) => { _logger.LogError("Error connecting to RabbitMQ. Retrying in 5 sec."); })
                    .Execute(() =>
                    {
                        var factory = new ConnectionFactory() { HostName = _options.Host,
                            UserName = _options.Username, Password = _options.Password
                        };
                        using (var connection = factory.CreateConnection())
                        {
                            using (var model = connection.CreateModel())
                            {
                                model.ExchangeDeclare(_options.Exchange, "fanout", durable: true, autoDelete: false);
                                string data = MessageSerializer.Serialize(message);
                                var body = Encoding.UTF8.GetBytes(data);
                                IBasicProperties properties = model.CreateBasicProperties();
                                properties.Headers = new Dictionary<string, object> { { "MessageType", messageType } };
                                model.BasicPublish(_options.Exchange, routingKey, properties, body);
                            }
                        }
                    }));
        }
    }
}
