using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mooshak_TestByE.Startup))]
namespace Mooshak_TestByE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
