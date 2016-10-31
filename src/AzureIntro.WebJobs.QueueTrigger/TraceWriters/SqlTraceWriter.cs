using Microsoft.Azure.WebJobs.Host;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace AzureIntro.WebJobs.QueueTrigger.TraceWriters
{
    public class SqlTraceWriter : TraceWriter
    {
        private string SqlConnectionString { get; set; }

        private string LogTableName { get; set; }

        public SqlTraceWriter(TraceLevel level, string sqlConnectionString, string logTableName)
            : base(level)
        {
            this.SqlConnectionString = sqlConnectionString;
            this.LogTableName = logTableName;
        }

        public override void Trace(TraceEvent traceEvent)
        {
            using (var sqlConnection = this.CreateConnection())
            {
                sqlConnection.Open();

                using (var cmd = new SqlCommand(string.Format("insert into {0} ([Source], [Timestamp], [Level], [Message], [Exception], [Properties]) values (@Source, @Timestamp, @Level, @Message, @Exception, @Properties)", this.LogTableName), sqlConnection))
                {
                    cmd.Parameters.AddWithValue("Source", traceEvent.Source ?? "");
                    cmd.Parameters.AddWithValue("Timestamp", traceEvent.Timestamp);
                    cmd.Parameters.AddWithValue("Level", traceEvent.Level.ToString());
                    cmd.Parameters.AddWithValue("Message", traceEvent.Message ?? "");
                    cmd.Parameters.AddWithValue("Exception", traceEvent.Exception?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("Properties", string.Join("; ", traceEvent.Properties.Select(x => x.Key + ", " + x.Value?.ToString()).ToList()) ?? "");

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(this.SqlConnectionString);
        }
    }
}