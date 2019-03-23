namespace MachineInfluxDataService
{
    public class Configuration
    {
        public string RabbitMqBrokerName { get; set; }
        public string RabbitMqQueueName { get; set; }
        public string RabbitMqHost { get; set; }
        public string RabbitMqUser { get; set; }
        public string RabbitMqPassword { get; set; }
        public int RabbitMqPrefetchCount { get; set; }
        public string InfluxEndpoint { get; set; }
        public string InfluxUserName { get; set; }
        public string InfluxPassword { get; set; }
    }
}