namespace MassTransitAbstractions
{
    public class MassTransitOptions
    {
        public enum Transport
        {
            RabbitMQ, AzureServiceBus, AmazonSQS, AmazonMQ, InMemory
        }
        public Transport TransportType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
