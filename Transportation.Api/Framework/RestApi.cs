using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Security;

namespace Transportation.Api.Framework
{
    public class RestApi : IHttpHandler
    {
        public static ICollection<Route> Routes { get; private set; }
        public static string RoutePrefix { get; set; }

        static RestApi()
        {
            Routes = new List<Route>();
            RoutePrefix = "";
        }

        public static void RegisterRoutes(Assembly assembly)
        {
            foreach (Type type in assembly.ExportedTypes)
            {
                RegisterRoutes(type);
            }
        }

        public static void RegisterRoutes(Type type)
        {
            IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttributes().Any(a => a.GetType() == typeof(RouteAttribute)));
            foreach (MethodInfo method in methods)
            {
                CustomAttributeData attribute = method.CustomAttributes.First(x => x.AttributeType == typeof(RouteAttribute));
                Route route = new Route { 
                    Method = method, 
                    Url = attribute.ConstructorArguments[1].Value.ToString(),
                    Verb = (HttpVerb) attribute.ConstructorArguments[0].Value,
                };
                Routes.Add(route);
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext httpContext)
        {
            string requestMethod = httpContext.Request.HttpMethod.ToUpper();
            HttpVerb verb;

            switch (requestMethod)
            {
                case "POST": verb = HttpVerb.Post; break;
                case "PUT": verb = HttpVerb.Put; break;
                case "DELETE": verb = HttpVerb.Delete; break;
                case "GET": verb = HttpVerb.Get; break;
                default: throw new InvalidOperationException();
            }

            Route route = RouteResolver.Resolve(verb, httpContext.Request.Url);

            if (route != null)
            {
                StreamReader reader = new StreamReader(httpContext.Request.InputStream);

                ApiContext apiContext = new ApiContext
                {
                    Uri = httpContext.Request.Url,
                    Route = route,
                };

                if ((route.Verb == HttpVerb.Post) && (route.Url.Contains("images") || route.Url.Contains("spss") && !route.Url.Contains("bulkimport") || route.Url.Contains("templates") || route.Url.Contains("importCSV")))
                {
                    apiContext.HttpFileCollection = new HttpFileCollectionWrapper(httpContext.Request.Files);
                }
                else
                {
                    apiContext.RequestBody = reader.ReadToEnd();
                }

                IDictionary<string, object> parameters = ParameterParser.Parse(apiContext);

                ClarityDB.CreateInstance();

                RestApiResult result;

                if (!Authentication.RouteRequiresAuthenticate(route) || Authentication.IsAuthenticated(route))
                {
                    result = route.Invoke(parameters);
                }
                else
                {
                    result = new RestApiResult { StatusCode = HttpStatusCode.Unauthorized };
                }

                ClarityDB.DestroyInstance();

                httpContext.Response.StatusCode = (int)result.StatusCode;

                if (result.Json != null)
                {
                    var settings = new JsonSerializerSettings { Formatting = Formatting.None };
                    string response = JsonConvert.SerializeObject(result.Json, Formatting.None, settings);
                    httpContext.Response.Write(response);
                }
            }
            else
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }

            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.Response.SuppressFormsAuthenticationRedirect = true;
        }
    }
}