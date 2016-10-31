using AzureIntro.AzureHelpers;
using AzureIntro.WebJobs.QueueTrigger.TraceWriters;
using Microsoft.Azure.WebJobs;
using System.Diagnostics;

namespace AzureIntro.WebJobs.QueueTrigger
{
    class Program
    {
        static void Main()
        {
            var azureConfig = new AzureConfiguration();

            var config = new JobHostConfiguration();
            config.JobActivator = NinjectJobActivator.Instance;
            config.DashboardConnectionString = azureConfig.GetConnectionString("AzureIntroStorageConnection");
            config.StorageConnectionString = azureConfig.GetConnectionString("AzureIntroStorageConnection");

            // Log Console.Out to SQL using custom TraceWriter
            // Note: Need to update default Microsoft.Azure.WebJobs package for config.Tracing.Tracers to be exposed/available
            config.Tracing.Tracers.Add(new SqlTraceWriter(
                TraceLevel.Info,
                azureConfig.GetConnectionString("AzureIntroSqlConnection"),
                azureConfig.GetAppSetting("WebJobLogTableName")));

            // Note: set the number of messages that are pulled of each queue by default.
            // These messages will be processed in parallel.
            // config.Queues.BatchSize = 10;

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}