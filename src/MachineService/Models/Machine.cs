namespace MachineService.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Timers;
    using EventBus;
    using global::Models;
    using global::Models.IntegrationEvents;
    using MachineService.Core;

    public class Machine : IMachine, IDisposable
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

        public IEnumerable<MachineValue> LastMachineValues { get; private set; }

        public void StartMachine()
        {
            MachineStatus = MachineStatus.Running;
            _dataEventTimer.Start();
        }

        public void StopMachine()
        {
            MachineStatus = MachineStatus.Stopped;
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
                        var ts = DateTime.UtcNow;
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