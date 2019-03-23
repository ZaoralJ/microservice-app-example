namespace MachineService.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using MachineService.Core;
    using MachineService.Factories;

    public class MachineManager : IMachineManager
    {
        private readonly IMachineFactory _machineFactory;
        private Dictionary<string, IMachine> _machines;

        public MachineManager(IMachineFactory machineFactory, int machineCount)
        {
            _machineFactory = machineFactory;
            CreateMachines(machineCount);
        }

        public IMachine GetMachine(string machineName)
        {
            return _machines.TryGetValue(machineName, out var machine) ? machine : null;
        }

        public IEnumerable<IMachine> GetAllMachines()
        {
            return _machines.Select(x => x.Value);
        }

        private void CreateMachines(int machineCount)
        {
            _machines = Enumerable.Range(1, machineCount)
                                  .Select(m =>
                                      {
                                          var machine = _machineFactory.CreateMachine($"M{m}");
                                          machine.StartMachine();
                                          return machine;
                                      })
                                  .ToDictionary(k => k.MachineName, v => v);
        }
    }
}