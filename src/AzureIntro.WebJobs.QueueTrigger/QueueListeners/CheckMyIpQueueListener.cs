using AzureIntro.Models.StorageQueue;
using AzureIntro.WebJobs.QueueTrigger.Services;
using Microsoft.Azure.WebJobs;
using System;

namespace AzureIntro.WebJobs.QueueTrigger.QueueListeners
{
    public class CheckMyIpQueueListener
    {
        private IIpCheckerService IpCheckerService { get; set; }

        public CheckMyIpQueueListener(IIpCheckerService ipCheckerService)
        {
            this.IpCheckerService = ipCheckerService;
        }

        public async void ProcessCheckMyIpMessage([QueueTrigger("checkmyip")] CheckMyIpMessage message)
        {
            var ip = await this.IpCheckerService.GetIp();

            Console.WriteLine($"Requestor: {message.Requestor}; Current IP: {ip};");
        }
    }
}