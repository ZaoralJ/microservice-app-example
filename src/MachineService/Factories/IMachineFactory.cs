namespace MachineService.Factories
{
    using MachineService.Core;

    public interface IMachineFactory
    {
        IMachine CreateMachine(string machineName);
    }
}