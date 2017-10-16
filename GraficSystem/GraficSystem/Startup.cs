using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraficSystem.Startup))]
namespace GraficSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
