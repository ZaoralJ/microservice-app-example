namespace MachineService.Core
{
    using System.Collections.Generic;

    public interface IMachineManager
    {
        IMachine GetMachine(string machineName);
        IEnumerable<IMachine> GetAllMachines();
    }
}