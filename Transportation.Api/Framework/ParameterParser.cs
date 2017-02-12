using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Transportation.Api.Framework
{
    public static class ParameterParser
    {
        public static IDictionary<string, object> Parse(ApiContext context)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            ParsePath(dictionary, context);
            ParseQuery(dictionary, context);
            ParseBody(dictionary, context);
            ParseHttpFileCollection(dictionary, context);

            return dictionary;
        }

        private static void ParsePath(Dictionary<string, object> dictionary, ApiContext context)
        {
            if (context.Route.Url.Contains("{id}") || context.Route.Url.Contains("{path}") || context.Route.Url.Contains("{email}")
                 || context.Route.Url.Contains("{username}") || context.Route.Url.Contains("{code}") || context.Route.Url.Contains("{domainName}")
                || context.Route.Url.Contains("{secret}"))
            {
                List<string> requestSegments = RouteResolver.GetSignificantSegments(context.Uri.Segments);

                if (requestSegments.Contains("Epinion.Clarity.Web_deploy"))
                    requestSegments.Remove("Epinion.Clarity.Web_deploy");

                List<string> routeSegments = RouteResolver.GetSignificantSegments(context.Route.Url.Split('/'));

                for (int i = 0; i < routeSegments.Count; i++)
                {
                    if (routeSegments[i] == "{id}")
                    {
                        long id;
                        if (!long.TryParse(requestSegments[i], out id))
                        {
                            id = -100000;
                            throw new System.FormatException();
                        }
                        dictionary.Add("id", id);
                    }
                    else if (routeSegments[i] == "{path}")
                    {
                        string path = requestSegments[i];
                        dictionary.Add("path", path);
                    }
                    else if (routeSegments[i] == "{email}")
                    {
                        string path = requestSegments[i];
                        dictionary.Add("email", path);
                    }
                    else if (routeSegments[i] == "{username}")
                    {
                        string path = requestSegments[i];
                        dictionary.Add("username", path);
                    }
                    else if (routeSegments[i] == "{code}")
                    {
                        string path = requestSegments[i];
                        dictionary.Add("code", path);
                    }
                    else if (routeSegments[i] == "{domainName}")
                    {
                        string path = requestSegments[i];
                        dictionary.Add("domainName", path);
                    }
                    else if (routeSegments[i] == "{secret}")
                    {
                        string path = requestSegments[i];
                        dictionary.Add("secret", path);
                    }
                }
            }
        }

        private static void ParseQuery(Dictionary<string, object> dictionary, ApiContext context)
        {
            if (!string.IsNullOrEmpty(context.Uri.Query))
            {
                string[] parameterArray = context.Uri.Query.Replace("?", "").Split('&');

                foreach (string parameterString in parameterArray)
                {
                    string[] parameterKeyValue = parameterString.Split('=');
                    dictionary.Add(parameterKeyValue[0], HttpUtility.UrlDecode(parameterKeyValue[1]));
                }
            }
        }

        private static void ParseBody(Dictionary<string, object> dictionary, ApiContext context)
        {
            if (!string.IsNullOrEmpty(context.RequestBody))
            {
                dictionary.Add("json", JObject.Parse(context.RequestBody));
            }
        }

        private static void ParseHttpFileCollection(Dictionary<string, object> dictionary, ApiContext context)
        {
            if (context.HttpFileCollection != null)
            {
                dictionary.Add("httpFileCollection", context.HttpFileCollection);
            }
        }
    }
}
