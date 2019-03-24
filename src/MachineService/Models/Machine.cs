namespace MachineService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;
    using EventBus;
    using global::Models;
    using global::Models.IntegrationEvents;
    using MachineService.Core;

    public class Machine : IMachine
    {
        private readonly IEventBus _eventBus;
        private Timer _dataEventTimer;

        public Machine(string machineName, IEventBus eventBus)
        {
            _eventBus = eventBus;
            MachineName = machineName;
            StartMachineReporting();
        }

        public string MachineName { get; }

        public MachineStatus MachineStatus { get; private set; }

        public int OrderNumber { get; private set; }

        public IEnumerable<MachineValue> LastMachineValues { get; private set; }

        public void StartMachine(bool publishMessage)
        {
            MachineStatus = MachineStatus.Running;
            _dataEventTimer.Start();

            if (publishMessage)
            {
                _eventBus.Publish(new MachineStatusIntegrationEvent
                {
                    MachineName = MachineName,
                    MachineStatus = MachineStatus,
                    Description = $"Machine {MachineName} status changed to Running"
                });
            }
        }

        public void StartMachine(Guid parentId)
        {
            StartMachine(false);

            _eventBus.Publish(new MachineStatusIntegrationEvent(parentId)
            {
                MachineName = MachineName,
                MachineStatus = MachineStatus,
                Description = $"Machine {MachineName} status changed to Running"
            });
        }

        public void StopMachine(bool publishMessage)
        {
            MachineStatus = MachineStatus.Stopped;

            if (publishMessage)
            {
                _eventBus.Publish(new MachineStatusIntegrationEvent
                {
                    MachineName = MachineName,
                    MachineStatus = MachineStatus,
                    Description = $"Machine {MachineName} status changed to Running"
                });
            }
        }

        public void StopMachine(Guid parentId)
        {
            StopMachine(false);

            _eventBus.Publish(new MachineStatusIntegrationEvent(parentId)
            {
                MachineName = MachineName,
                MachineStatus = MachineStatus,
                Description = $"Machine {MachineName} status changed to Stopped"
            });
        }

        public async Task StartNewOrder(int orderNumber, Guid parentId)
        {
            StopMachine(parentId);

            await Task.Delay(3).ConfigureAwait(false);

            OrderNumber = orderNumber;

            StartMachine(parentId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dataEventTimer.Stop();
                _dataEventTimer.Dispose();
            }
        }

        private void StartMachineReporting()
        {
            var interval = new Random().Next(8, 14);
            _dataEventTimer = new Timer(interval * 1000);

            var r = new Random();

            _dataEventTimer.Elapsed += (sender, args) =>
                {
                    if (MachineStatus == MachineStatus.Running)
                    {
                        var ts = DateTime.Now;
                        var machineEvent = new MachineDataIntegrationEvent
                        {
                            MachineName = MachineName,
                            MachineValues = Enumerable.Range(1, 5)
                                                      .Select(v => new MachineValue
                                                      {
                                                          Name = $"Value{v}",
                                                          Value = r.Next(v + 10, v + 15),
                                                          TimeStamp = ts
                                                      }).ToList()
                        };

                        LastMachineValues = machineEvent.MachineValues;

                        _eventBus.Publish(machineEvent);
                    }
                };
        }
    }
}