using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Api.Framework
{
    public class RouteResolver
    {
        public static Route Resolve(HttpVerb verb, Uri uri)
        {
            List<string> uriSegments = GetSignificantSegments(uri.Segments);

            // TODO: remove segments before /api
            if (uriSegments.Contains("Epinion.Clarity.Web_deploy"))
                uriSegments.Remove("Epinion.Clarity.Web_deploy");
           
            return RestApi.Routes.FirstOrDefault(x => CompareSegments(uriSegments, GetFormattedUrl(RestApi.RoutePrefix + x.Url)) && x.Verb == verb);
        }

        private static bool CompareSegments(List<string> requestSegments, string routeUrl)
        {
            List<string> routeSegments = GetSignificantSegments(routeUrl.Split('/'));

            if (routeSegments.Count != requestSegments.Count) return false;

            for (int i = 0; i < routeSegments.Count; i++)
            {
                if (!routeSegments[i].Contains('{'))
                {
                    if (routeSegments[i].ToLower() != requestSegments[i].ToLower())
                        return false;
                }
            }

            return true;
        }

        private static string GetFormattedUrl(string url)
        {
            if (url.EndsWith("/"))
            {
                return url.Substring(0, url.Length - 1);
            }
            return url;
        }

        public static List<string> GetSignificantSegments(string[] segments)
        {
            List<string> result = new List<string>();

            foreach (string s in segments)
            {
                if (s.Replace("/", "") != RestApi.RoutePrefix.Replace("/", "")
                    && s != "/"
                    && !string.IsNullOrEmpty(s))
                { 
                    result.Add(s.Replace("/", ""));
                }
            }

            return result;
        }
    }
}
