using Transportation.Api.Framework;
using System.Reflection;

namespace Transportation.Web
{
    public class Application : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(RestApi));
            RestApi.RegisterRoutes(assembly);
            RestApi.RoutePrefix= "/api";
        }
   }
}