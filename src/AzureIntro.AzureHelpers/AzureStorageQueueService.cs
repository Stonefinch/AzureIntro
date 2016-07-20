using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureIntro.AzureHelpers
{
    public interface IAzureStorageQueueService
    {
        void EnqueueMessage(string queueName, string jsonMessage);
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

        private CloudQueueClient GetCloudQueueClient()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(this.StorageAccountConnectionString);

            return cloudStorageAccount.CreateCloudQueueClient();
        }
    }
}