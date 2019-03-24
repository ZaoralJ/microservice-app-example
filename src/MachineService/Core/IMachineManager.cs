namespace MachineService.Core
{
    using System;
    using System.Collections.Generic;

    public interface IMachineManager : IDisposable
    {
        IMachine GetMachine(string machineName);
        IEnumerable<IMachine> GetAllMachines();
    }
}