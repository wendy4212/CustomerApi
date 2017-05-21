using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CustomerApi.Startup))]
namespace CustomerApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
