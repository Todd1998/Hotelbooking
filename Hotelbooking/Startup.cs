using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hotelbooking.Startup))]
namespace Hotelbooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
