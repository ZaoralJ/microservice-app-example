namespace MachineCommandService.Controllers
{
    using EventBus;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.IntegrationEvents;

    [Route("api/machinecommand")]
    [ApiController]
    public class MachineCommandController : Controller
    {
        private readonly IEventBus _eventBus;

        public MachineCommandController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost("{machineName}/start")]
        public IActionResult StartMachine(string machineName)
        {
            var ie = new MachineCommandIntegrationEvent
            {
                MachineName = machineName,
                MachineStatus = MachineStatus.Running,
                Description = $"Stop machine {machineName}"
            };

            _eventBus.Publish(ie);

            return Ok();
        }

        [HttpPost("{machineName}/stop")]
        public IActionResult StopMachine(string machineName)
        {
            var ie = new MachineCommandIntegrationEvent
            {
                MachineName = machineName,
                MachineStatus = MachineStatus.Stopped,
                Description = $"Start machine {machineName}"
            };

            _eventBus.Publish(ie);

            return Ok();
        }

        [HttpPost("{machineName}/order/{orderNumber}")]
        public IActionResult SetOrderMachine(string machineName, int orderNumber)
        {
            var ie = new MachineCommandIntegrationEvent
            {
                MachineName = machineName,
                OrderNumber = orderNumber,
                Description = $"New order number {orderNumber} on machine {machineName}"
            };

            _eventBus.Publish(ie);

            return Ok();
        }
    }
}