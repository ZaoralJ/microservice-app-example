namespace MachineService
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventBus;

    using global::Models.IntegrationEvents;

    using MachineService.Core;
    using MachineService.EventHandlers;

    using Microsoft.Extensions.Hosting;

    public class Service : IHostedService
    {
        private readonly IMachineManager _machineManager;
        private readonly IEventBus _eventBus;

        public Service(IMachineManager machineManager, IEventBus eventBus)
        {
            _machineManager = machineManager;
            _eventBus = eventBus;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _eventBus.Subscribe<MachineCommandIntegrationEvent, MachineCommandIntegrationEventHandler>();
            _eventBus.StartConsumerChannel();

            foreach (var machine in _machineManager.GetAllMachines())
            {
                machine.StartMachine(false);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _machineManager.Dispose();

            return Task.CompletedTask;
        }
    }
}