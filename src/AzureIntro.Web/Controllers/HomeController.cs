using AzureIntro.AzureHelpers;
using AzureIntro.Models.StorageQueue;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace AzureIntro.Web.Controllers
{
    public class HomeController : Controller
    {
        private IAzureStorageQueueService _azureStorageQueueService { get; set; }

        public HomeController()
        {
            var config = new AzureConfiguration();

            _azureStorageQueueService = new AzureStorageQueueService(config.GetConnectionString("AzureIntroStorageConnection"));
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }

        public JsonResult StartWebJobTask(string requestor)
        {
            // a request was made that will kick off a long running process.
            // add a message to a queue to be processed asynchronously 
            var json = JsonConvert.SerializeObject(new CheckMyIpMessage() { Requestor = requestor });
            _azureStorageQueueService.EnqueueMessage("checkmyip", json);

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}