namespace MachineService
{
    using System.Threading;
    using System.Threading.Tasks;
    using MachineService.Core;
    using Microsoft.Extensions.Hosting;

    public class Service : IHostedService
    {
        private readonly IMachineManager _machineManager;

        public Service(IMachineManager machineManager)
        {
            _machineManager = machineManager;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}