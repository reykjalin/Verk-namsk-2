using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JT_TestProject.Startup))]
namespace JT_TestProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
