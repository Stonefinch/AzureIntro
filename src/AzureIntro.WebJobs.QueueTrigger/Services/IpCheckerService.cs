using System.Net.Http;
using System.Threading.Tasks;

namespace AzureIntro.WebJobs.QueueTrigger.Services
{
    public interface IIpCheckerService
    {
        Task<string> GetIp();
    }

    public class IpCheckerService : IIpCheckerService
    {
        public IpCheckerService()
        {
            // no-op
        }

        public async Task<string> GetIp()
        {
            var client = new HttpClient();
            var resp = await client.GetAsync("http://icanhazip.com/");

            return await resp.Content.ReadAsStringAsync();
        }
    }
}