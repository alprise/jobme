using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobMe.Web.Mvc.Startup))]
namespace JobMe.Web.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
