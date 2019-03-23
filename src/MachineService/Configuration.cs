namespace MachineService
{
    public class Configuration
    {
        public string RabbitMqBrokerName { get; set; }
        public string RabbitMqHost { get; set; }
        public string RabbitMqUser { get; set; }
        public string RabbitMqPassword { get; set; }
        public int MachineCount { get; set; }
    }
}