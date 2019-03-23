namespace MachineSqlDataService
{
    using System;
    using System.Threading.Tasks;
    using EventBus;
    using Interfaces;
    using Models.IntegrationEvents;

    public class MachineDataIntegrationEventHandler : IntegrationEventHandlerBase<MachineDataIntegrationEvent>
    {
        private readonly IWriteMachineDataRepository _writeMachineDataRepository;

        public MachineDataIntegrationEventHandler(
            IWriteMachineDataRepository writeMachineDataRepository,
            IEventLogger eventLogger) : base(eventLogger)
        {
            _writeMachineDataRepository = writeMachineDataRepository ?? throw new ArgumentNullException(nameof(writeMachineDataRepository));
        }

        protected override Task HandleBody(MachineDataIntegrationEvent integrationEvent)
        {
            return _writeMachineDataRepository.WriteMachineValuesAsync(integrationEvent.MachineName, integrationEvent.MachineValues);
        }
    }
}