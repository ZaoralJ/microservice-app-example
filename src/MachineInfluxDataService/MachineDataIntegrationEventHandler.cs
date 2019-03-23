namespace MachineInfluxDataService
{
    using System;
    using System.Threading.Tasks;
    using EventBus;
    using Interfaces;
    using Models.IntegrationEvents;

    public class MachineDataIntegrationEventHandler : IIntegrationEventHandler<MachineDataIntegrationEvent>
    {
        private readonly IWriteMachineDataRepository _writeMachineDataRepository;

        public MachineDataIntegrationEventHandler(IWriteMachineDataRepository writeMachineDataRepository)
        {
            _writeMachineDataRepository = writeMachineDataRepository ?? throw new ArgumentNullException(nameof(writeMachineDataRepository));
        }

        public Task Handle(MachineDataIntegrationEvent integrationEvent)
        {
            return _writeMachineDataRepository.WriteMachineValuesAsync(integrationEvent.MachineName, integrationEvent.MachineValues);
        }
    }
}