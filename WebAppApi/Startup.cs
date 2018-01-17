using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Newtonsoft.Json.Converters;
using Owin;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(WebAppApi.Startup))]
namespace WebAppApi
{

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }

}