using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WineVaultApplication.Startup))]
namespace WineVaultApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
