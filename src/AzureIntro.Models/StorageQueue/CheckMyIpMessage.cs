namespace AzureIntro.Models.StorageQueue
{
    public class CheckMyIpMessage : StorageQueueMessageBase
    {
        public string Requestor { get; set; }
    }
}