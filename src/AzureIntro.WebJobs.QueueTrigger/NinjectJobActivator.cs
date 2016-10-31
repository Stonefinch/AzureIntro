using AzureIntro.WebJobs.QueueTrigger.Services;
using Microsoft.Azure.WebJobs.Host;
using Ninject;

namespace AzureIntro.WebJobs.QueueTrigger
{
    public class NinjectJobActivator : IJobActivator
    {
        public static NinjectJobActivator Instance
        {
            get { return _instance ?? (_instance = new NinjectJobActivator()); }
        }
        private static NinjectJobActivator _instance;

        private readonly IKernel _kernel;

        public NinjectJobActivator()
        {
            _kernel = new StandardKernel();

            // define job dependencies
            _kernel.Bind<IIpCheckerService>().To<IpCheckerService>();
        }

        public T CreateInstance<T>()
        {
            return this._kernel.Get<T>();
        }
    }
}
