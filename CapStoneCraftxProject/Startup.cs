using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CapStoneCraftxProject.Startup))]
namespace CapStoneCraftxProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
