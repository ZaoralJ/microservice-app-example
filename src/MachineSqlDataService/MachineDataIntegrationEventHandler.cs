namespace MachineSqlDataService
{
    using System;
    using System.Threading.Tasks;
    using EventBus;
    using Interfaces;
    using Logging;
    using Models.IntegrationEvents;
    using Polly;

    public class MachineDataIntegrationEventHandler : IntegrationEventHandlerBase<MachineDataIntegrationEvent>
    {
        private readonly IWriteMachineDataRepository _writeMachineDataRepository;
        private readonly ILogger _logger;

        public MachineDataIntegrationEventHandler(
            IWriteMachineDataRepository writeMachineDataRepository,
            IEventLogger eventLogger,
            ILogger logger) :
            base(eventLogger)
        {
            _writeMachineDataRepository = writeMachineDataRepository ?? throw new ArgumentNullException(nameof(writeMachineDataRepository));
            _logger = logger;
        }

        protected override async Task HandleBody(MachineDataIntegrationEvent integrationEvent)
        {
            var policy = Policy.Handle<Exception>()
                               .WaitAndRetryAsync(
                                   3,
                                   retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                                   (ex, time) => { _logger.Warn(ex); });

            await policy.ExecuteAsync(() => _writeMachineDataRepository.WriteMachineValuesAsync(integrationEvent.MachineName, integrationEvent.MachineValues));
        }
    }
}