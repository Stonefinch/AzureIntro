using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureIntro.Web.Startup))]
namespace AzureIntro.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
