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

        public async Task<IEnumerable<MachineValue>> LastMachineValuesAsync(string machineName)
        {
            var query = $"SELECT * FROM machinedata WHERE machinename='{machineName}' GROUP BY * ORDER BY DESC LIMIT 1";
            var data = await _influxDb.QueryAsync("machinedata", query);

            var row = data.FirstOrDefault();

            if (row == null)
            {
                return null;
            }

            var res = new List<MachineValue>();

            for (var i = 1; i < row.Columns.Length; i++)
            {
                res.Add(new MachineValue
                {
                    Name = row.Columns[i],
                    Value = row.Values[0][i],
                    TimeStamp = (DateTime)row.Values[0][0]
                });
            }

            return res;
        }

        public async Task<IEnumerable<MachineValue>> ReadMachineValuesAsync(string machineName, DateTime from, DateTime to)
        {
            var query = $"SELECT Value1, Value2, Value3, Value4, Value5 FROM machinedata WHERE machinename='{machineName}' AND time>='{from:yyyy-MM-dd HH:mm:ss.fff}' AND time<='{to:yyyy-MM-dd HH:mm:ss.fff}' ORDER BY time";
            var data = await _influxDb.QueryAsync("machinedata", query);
            
            var serie = data.FirstOrDefault();

            if (serie == null)
            {
                return Enumerable.Empty<MachineValue>();
            }

            var res = new List<MachineValue>();

            foreach (var row in serie.Values)
            {
                for (var i = 1; i < serie.Columns.Length; i++)
                {
                    res.Add(new MachineValue
                    {
                        Name = serie.Columns[i],
                        Value = row[i],
                        TimeStamp = (DateTime)row[0]
                    });
                }
            }

            return res;
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