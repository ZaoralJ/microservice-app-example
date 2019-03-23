namespace Models.IntegrationEvents
{
    using System.Collections.Generic;
    using EventBus.Events;

    public class MachineDataIntegrationEvent : IntegrationEvent
    {
        public string MachineName { get; set; }
        public List<MachineValue> MachineValues { get; set; }
    }
}
