using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Transportation.Api.Framework
{
    public class RouteAttribute : Attribute
    {
        public HttpVerb HttpVerb { get; set; }
        public Uri Uri { get; set; }

        public RouteAttribute(HttpVerb httpVerb, string url)
        {
            this.HttpVerb = httpVerb;
        }
    }
}