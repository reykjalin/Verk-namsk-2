using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mooshak2_FirstTest_KR.Startup))]
namespace Mooshak2_FirstTest_KR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
