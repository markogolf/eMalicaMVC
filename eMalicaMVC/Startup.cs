using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eMalicaMVC.Startup))]
namespace eMalicaMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
