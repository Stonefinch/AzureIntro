using AzureIntro.Models.StorageQueue;
using AzureIntro.WebJobs.QueueTrigger.Services;
using Microsoft.Azure.WebJobs;
using System;

namespace AzureIntro.WebJobs.QueueTrigger.QueueListeners
{
    public class CheckMyIpQueueListener
    {
        private IIpCheckerService IpCheckerService { get; set; }

        // Note: Dependency Injection is made possible by overriding the JobActivator in the JobHostConfiguration of the JobHost.
        public CheckMyIpQueueListener(IIpCheckerService ipCheckerService)
        {
            this.IpCheckerService = ipCheckerService;
        }

        // Note: the message param is strongly typed. The Json message is parsed into the complex type by the WebJob SDK.
        public async void ProcessCheckMyIpMessage([QueueTrigger("checkmyip")] CheckMyIpMessage message)
        {
            var ip = await this.IpCheckerService.GetIp();

            Console.WriteLine($"Requestor: {message.Requestor}; Current IP: {ip};");
        }
    }
}