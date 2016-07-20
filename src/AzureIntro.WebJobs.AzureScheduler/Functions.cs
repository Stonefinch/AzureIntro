using AzureIntro.AzureHelpers;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Scheduler.Models;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace AzureIntro.WebJobs.AzureScheduler
{
    // note: The name of this class can/should be updated, it will be used in logs displayed via the kudu dashboard.
    public class Functions
    {
        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called azurescheduler.
        // Note: This name of this method can/should be updated, it will be used in logs displayed via the kudu dashboard.
        // The WebJob SDK registers all methods that include the QueueTrigger parameter all calls them when a new message appears in the specified queue.
        public void ProcessQueueMessage([QueueTrigger("azurescheduler")] string message, TextWriter log)
        {
            // Note: The Azure Scheduler can not drop a json message into a queue, only an XML message.
            // All this WebJob does is read the XML message out of one queue (the payload of which is a json string)
            // and writes that message to a different queue.

            // note: Logs written to the TextWriter will be saved in the AzureStorage account and displayed via the kudu dashboard.
            // Logs written to the console will not by default. (see project AzureWebJobLogsToSql for solution)
            log.WriteLine($"textWriter: message received: {message}");
            Console.WriteLine($"console: message received: {message}");

            var storageQueueMessage = this.DeserializeStorageQueueMessage(message);

            // Note: We will inject dependencies in the WebJobQueue project, just new these up here for this demo.
            var config = new AzureConfiguration();
            var queueService = new AzureStorageQueueService(config.GetConnectionString("AzureWebJobsStorage"));

            queueService.EnqueueMessage("webjobqueue", storageQueueMessage.Message);
        }

        private StorageQueueMessage DeserializeStorageQueueMessage(string message)
        {
            var encoding = this.GetEncoding(message);

            StorageQueueMessage result = null;

            var xmlSerializer = new XmlSerializer(typeof(StorageQueueMessage));
            using (var ms = new MemoryStream(encoding.GetBytes(message)))
            {
                result = (StorageQueueMessage)xmlSerializer.Deserialize(ms);
            }

            return result;
        }

        private Encoding GetEncoding(string xml)
        {
            if (xml.Contains(@"encoding=""utf-16"""))
                return Encoding.Unicode;

            if (xml.Contains(@"encoding=""utf-8"""))
                return Encoding.UTF8;

            throw new Exception("XML string Encoding unknown.");
        }
    }
}