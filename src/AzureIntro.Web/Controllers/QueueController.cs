using AzureIntro.AzureHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AzureIntro.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class QueueController : Controller
    {
        private IAzureStorageQueueService _azureStorageQueueService { get; set; }

        public QueueController()
        {
            var config = new AzureConfiguration();

            _azureStorageQueueService = new AzureStorageQueueService(config.GetConnectionString("FunctionIntroStorageConnection"));
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult QueueMessages(string queueName, double messageCount, int threadCount)
        {
            if (threadCount <= 0 || threadCount > 100)
                threadCount = 1;
            
            var messagesPerThread = (int)Math.Floor(messageCount / threadCount);

            Parallel.For(0, threadCount, i =>
            {
                var jsonMessages = Enumerable.Range(0, messagesPerThread).Select(x => $"{{\"Id\":\"{Guid.NewGuid()}\"}}").ToList();
                this._azureStorageQueueService.EnqueueMessages(queueName, jsonMessages);
            });

            return Json(new { message = "success" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult QueueStats(string queueName)
        {
            return Json(new { message = _azureStorageQueueService.GetQueueStats(queueName) }, JsonRequestBehavior.AllowGet);
        }
    }
}