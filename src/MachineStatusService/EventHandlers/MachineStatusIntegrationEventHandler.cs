namespace MachineStatusService.EventHandlers
{
    using System.Threading.Tasks;
    using EventBus;
    using Models.IntegrationEvents;

    public class MachineStatusIntegrationEventHandler : IntegrationEventHandlerBase<MachineStatusIntegrationEvent>
    {
        public MachineStatusIntegrationEventHandler(IEventLogger eventLogger)
            : base(eventLogger)
        {
        }

        protected override Task HandleBody(MachineStatusIntegrationEvent integrationEvent)
        {
            return Task.CompletedTask;
        }
    }
}