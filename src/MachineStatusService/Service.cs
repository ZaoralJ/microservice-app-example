namespace MachineStatusService
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventBus;
    using MachineStatusService.EventHandlers;
    using Microsoft.Extensions.Hosting;
    using Models.IntegrationEvents;

    public class Service : IHostedService
    {
        private readonly IEventBus _eventBus;

        public Service(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<MachineStatusIntegrationEvent, MachineStatusIntegrationEventHandler>();
            _eventBus.StartConsumerChannel();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}