namespace Data.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Interfaces;
    using Models;

    public class MachineDataRepository : IReadMachineDataRepository, IWriteMachineDataRepository
    {
        private readonly string _connectionString;

        public MachineDataRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<MachineValue>> LastMachineValuesAsync(string machineName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var record = await connection.QuerySingleOrDefaultAsync<DbRecord>("dbo.MachineDataLatestSelect", param: new { machineName }, commandType: CommandType.StoredProcedure);

                return GetMachineValues(record);
            }
        }

        public async Task<IEnumerable<MachineValue>> ReadMachineValuesAsync(string machineName, DateTime from, DateTime to)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var records = await connection.QueryAsync<DbRecord>("dbo.MachineDataSelect", param: new { machineName, from, to }, commandType: CommandType.StoredProcedure);

                return records.SelectMany(GetMachineValues);
            }
        }

        public async Task WriteMachineValuesAsync(string machineName, IEnumerable<MachineValue> machineValues)
        {
            var data = from mv in machineValues
                       group mv by mv.TimeStamp into g
                       select new
                       {
                           MachineName = machineName,
                           TimeStamp = g.Key,
                           Value1 = GetValue(g.ToList(), "Value1"),
                           Value2 = GetValue(g.ToList(), "Value2"),
                           Value3 = GetValue(g.ToList(), "Value3"),
                           Value4 = GetValue(g.ToList(), "Value4"),
                           Value5 = GetValue(g.ToList(), "Value5")
                       };

            using (var connection = new SqlConnection(_connectionString))
            {
                foreach (var rec in data)
                {
                    await connection.ExecuteAsync("dbo.MachineDataInsert", param: rec, commandType: CommandType.StoredProcedure);
                }
            }

            int? GetValue(IEnumerable<MachineValue> mv, string valueName)
            {
                var v = mv.FirstOrDefault(x => x.Name == valueName)?.Value;

                if (v == null)
                {
                    return null;
                }

                return Convert.ToInt32(v);
            }
        }

        private IEnumerable<MachineValue> GetMachineValues(DbRecord record)
        {
            if (record == null)
            {
                return null;
            }

            var res = new List<MachineValue>
              {
                  new MachineValue { TimeStamp = record.TimeStamp, Name = "Value1", Value = record.Value1 },
                  new MachineValue { TimeStamp = record.TimeStamp, Name = "Value2", Value = record.Value2 },
                  new MachineValue { TimeStamp = record.TimeStamp, Name = "Value3", Value = record.Value3 },
                  new MachineValue { TimeStamp = record.TimeStamp, Name = "Value4", Value = record.Value4 },
                  new MachineValue { TimeStamp = record.TimeStamp, Name = "Value5", Value = record.Value5 }
              };

            return res;
        }

        private class DbRecord
        {
            public string MachineName { get; set; }
            public DateTime TimeStamp { get; set; }
            public int? Value1 { get; set; }
            public int? Value2 { get; set; }
            public int? Value3 { get; set; }
            public int? Value4 { get; set; }
            public int? Value5 { get; set; }
        }
    }
}