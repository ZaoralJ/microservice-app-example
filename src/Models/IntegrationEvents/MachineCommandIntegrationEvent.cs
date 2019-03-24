namespace Models.IntegrationEvents
{
    using EventBus.Events;

    public class MachineCommandIntegrationEvent : IntegrationEvent
    {
        public string MachineName { get; set; }
        public int OrderNumber { get; set; }
        public MachineStatus MachineStatus { get; set; }
    }
}