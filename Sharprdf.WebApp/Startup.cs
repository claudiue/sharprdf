using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sharprdf.WebApp.Startup))]
namespace Sharprdf.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
