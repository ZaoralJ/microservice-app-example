namespace MachineInfluxDataService
{
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Influx;
    using EventBus;
    using Microsoft.Extensions.Hosting;
    using Models.IntegrationEvents;

    public class Service : IHostedService
    {
        private readonly IEventBus _eventBus;
        private readonly IConfigureRepository _configureRepository;

        public Service(IEventBus eventBus, IConfigureRepository configureRepository)
        {
            _eventBus = eventBus;
            _configureRepository = configureRepository;
            _eventBus.Subscribe<MachineDataIntegrationEvent, MachineDataIntegrationEventHandler>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _configureRepository.Configure();
            _eventBus.StartConsumerChannel();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}