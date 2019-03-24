namespace Models.IntegrationEvents
{
    using System;
    using EventBus.Events;

    public class MachineStatusIntegrationEvent : IntegrationEvent
    {
        public MachineStatusIntegrationEvent()
        {
        }

        public MachineStatusIntegrationEvent(Guid parentId)
            : base(parentId)
        {
        }

        public string MachineName { get; set; }
        public MachineStatus MachineStatus { get; set; }
    }
}