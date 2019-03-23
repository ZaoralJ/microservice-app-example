namespace MachineService.Core
{
    using System.Collections.Generic;
    using global::Models;

    public interface IMachine
    {
        string MachineName { get; }
        MachineStatus MachineStatus { get; }
        IEnumerable<MachineValue> LastMachineValues { get; }
        void StartMachine();
        void StopMachine();
    }
}