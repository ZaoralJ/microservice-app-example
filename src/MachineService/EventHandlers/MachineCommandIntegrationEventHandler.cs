namespace MachineService.EventHandlers
{
    using System.Threading.Tasks;
    using EventBus;
    using global::Models;
    using global::Models.IntegrationEvents;
    using Logging;
    using MachineService.Core;

    public class MachineCommandIntegrationEventHandler : IntegrationEventHandlerBase<MachineCommandIntegrationEvent>
    {
        private readonly IMachineManager _machineManager;
        private readonly IEventBus _eventBus;
        private readonly ILogger _logger;

        public MachineCommandIntegrationEventHandler(
            IMachineManager machineManager,
            IEventBus eventBus,
            IEventLogger eventLogger,
            ILogger logger)
            : base(eventLogger)
        {
            _machineManager = machineManager;
            _eventBus = eventBus;
            _logger = logger;
        }

        protected override async Task HandleBody(MachineCommandIntegrationEvent integrationEvent)
        {
            var machine = _machineManager.GetMachine(integrationEvent.MachineName);

            if (machine == null)
            {
                _logger.Warn($"Can't find machine {integrationEvent.MachineName}");
                return;
            }

            if (integrationEvent.OrderNumber != 0)
            {
                await machine.StartNewOrder(integrationEvent.OrderNumber, integrationEvent.Id).ConfigureAwait(false);
            }
            else
            {
                switch (integrationEvent.MachineStatus)
                {
                    case MachineStatus.Running:
                        machine.StartMachine(false);

                        _eventBus.Publish(new MachineStatusIntegrationEvent(integrationEvent.Id)
                        {
                            MachineName = machine.MachineName,
                            MachineStatus = machine.MachineStatus,
                            Description = $"Machine {integrationEvent.MachineName} status changed to Running"
                        });

                        break;
                    case MachineStatus.Stopped:
                        machine.StopMachine(false);

                        _eventBus.Publish(new MachineStatusIntegrationEvent(integrationEvent.Id)
                        {
                            MachineName = machine.MachineName,
                            MachineStatus = machine.MachineStatus,
                            Description = $"Machine {integrationEvent.MachineName} status changed to Stopped"
                        });
                        break;
                }
            }
        }
    }
}