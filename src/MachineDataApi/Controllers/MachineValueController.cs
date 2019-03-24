namespace MachineDataApi.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class MachineValueController : ControllerBase
    {
        private readonly IReadMachineDataRepository _readMachineDataRepository;

        public MachineValueController(IReadMachineDataRepository readMachineDataRepository)
        {
            _readMachineDataRepository = readMachineDataRepository ?? throw new ArgumentNullException(nameof(readMachineDataRepository));
        }

        [HttpGet("{machineName}/lastvalues")]
        public async Task<IActionResult> MachineLastValues(string machineName)
        {
            return Ok(await _readMachineDataRepository.LastMachineValuesAsync(machineName));
        }

        [HttpGet("{machineName}/{from}/{to}")]
        public async Task<IActionResult> MachineLastValues(string machineName, DateTime from, DateTime to)
        {
            return Ok(await _readMachineDataRepository.ReadMachineValuesAsync(machineName, from, to));
        }
    }
}