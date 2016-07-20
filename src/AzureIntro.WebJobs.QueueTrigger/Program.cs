using AzureIntro.AzureHelpers;
using Microsoft.Azure.WebJobs;

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
            
            // Note: set the number of messages that are pulled of each queue by default.
            // These messages will be processed in parallel.
            // config.Queues.BatchSize = 10;

            var host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}