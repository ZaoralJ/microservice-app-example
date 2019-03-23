namespace MachineSqlDataService
{
    using System.Threading.Tasks;
    using EventBus;
    using Models.IntegrationEvents;

    public class MachineDataIntegrationEventHandler : IIntegrationEventHandler<MachineDataIntegrationEvent>
    {
        public async Task Handle(MachineDataIntegrationEvent integrationEvent)
        {
            await Task.Delay(1);
        }
    }
}