using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Collections.Generic;

namespace AzureIntro.AzureHelpers
{
    public interface IAzureStorageQueueService
    {
        void EnqueueMessage(string queueName, string jsonMessage);

        void EnqueueMessages(string queueName, List<string> jsonMessages);

        string GetQueueStats(string queueName);
    }

    public class AzureStorageQueueService : IAzureStorageQueueService
    {
        private string StorageAccountConnectionString { get; set; }

        public AzureStorageQueueService(string storageAccountConnectionString)
        {
            this.StorageAccountConnectionString = storageAccountConnectionString;
        }

        public void EnqueueMessage(string queueName, string jsonMessage)
        {
            var queue = GetCloudQueueClient().GetQueueReference(queueName.ToLowerInvariant());

            queue.AddMessage(new CloudQueueMessage(jsonMessage));
        }

        public void EnqueueMessages(string queueName, List<string> jsonMessages)
        {
            var queue = GetCloudQueueClient().GetQueueReference(queueName.ToLowerInvariant());

            foreach (var jsonMessage in jsonMessages)
            {
                queue.AddMessage(new CloudQueueMessage(jsonMessage));
            }
        }

        public string GetQueueStats(string queueName)
        {
            var queue = GetCloudQueueClient().GetQueueReference(queueName.ToLowerInvariant());
            queue.FetchAttributes();

            var approxMessageCount = queue.ApproximateMessageCount ?? 0;

            return $"approxMessageCount: {approxMessageCount}";
        }

        private CloudQueueClient GetCloudQueueClient()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(this.StorageAccountConnectionString);

            return cloudStorageAccount.CreateCloudQueueClient();
        }
    }
}