namespace Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IWriteMachineDataRepository
    {
        Task WriteMachineValuesAsync(string machineName, IEnumerable<MachineValue> machineValues);
    }
}