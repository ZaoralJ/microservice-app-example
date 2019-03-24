namespace MachineService.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Models;

    public interface IMachine : IDisposable
    {
        string MachineName { get; }
        MachineStatus MachineStatus { get; }
        int OrderNumber { get; }
        IEnumerable<MachineValue> LastMachineValues { get; }
        void StartMachine(bool publishMessage);
        void StartMachine(Guid parentId);
        void StopMachine(bool publishMessage);
        void StopMachine(Guid parentId);
        Task StartNewOrder(int orderNumber, Guid parentId);
    }
}