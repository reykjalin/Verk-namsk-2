using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Spaghetti.Startup))]
namespace Spaghetti
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
