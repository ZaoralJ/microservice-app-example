namespace Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IReadMachineDataRepository
    {
        Task<IEnumerable<MachineValue>> LastMachineValuesAsync(string machineName);
        Task<IEnumerable<MachineValue>> ReadMachineValuesAsync(string machineName, DateTime from, DateTime to);
    }
}