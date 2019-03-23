namespace Data.Influx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using InfluxDB.Net;
    using InfluxDB.Net.Contracts;
    using InfluxDB.Net.Models;
    using Interfaces;
    using Models;

    public class MachineDataRepository : IReadMachineDataRepository, IWriteMachineDataRepository, IConfigureRepository
    {
        private readonly IInfluxDb _influxDb;

        public MachineDataRepository(string endpoint, string userName, string password)
        {
            _influxDb = new InfluxDb(endpoint, userName, password);
        }

        public Task<IEnumerable<MachineValue>> LastMachineValuesAsync(string machineName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MachineValue>> ReadMachineValuesAsync(string machineName, DateTime @from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public async Task WriteMachineValuesAsync(string machineName, IEnumerable<MachineValue> machineValues)
        {
            var points = (from mv in machineValues
                          group mv by mv.TimeStamp into g
                          select new Point
                          {
                              Measurement = "machinedata",
                              Tags = new Dictionary<string, object> { { "machinename", machineName } },
                              Fields = g.ToDictionary(k => k.Name, v => v.Value),
                              Timestamp = g.Key
                          }).ToArray();

            await _influxDb.WriteAsync("machinedata", points);
        }

        public async Task Configure()
        {
            var dbs = await _influxDb.ShowDatabasesAsync();

            if (dbs.All(x => x.Name != "machinedata"))
            {
                await _influxDb.CreateDatabaseAsync("machinedata");
            }
        }
    }
}