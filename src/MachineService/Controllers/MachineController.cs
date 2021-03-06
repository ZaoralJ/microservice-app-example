﻿namespace MachineService.Controllers
{
    using System;
    using MachineService.Core;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/machine")]
    [ApiController]
    public class MachineController : Controller
    {
        private readonly IMachineManager _machineManager;

        public MachineController(IMachineManager machineManager)
        {
            _machineManager = machineManager ?? throw new ArgumentNullException(nameof(machineManager));
        }

        [HttpGet("allmachines")]
        public IActionResult GetAllMachines()
        {
            return Ok(_machineManager.GetAllMachines());
        }

        [HttpGet("{machineName}")]
        public IActionResult GetMachine(string machineName)
        {
            return Ok(_machineManager.GetMachine(machineName));
        }
    }
}