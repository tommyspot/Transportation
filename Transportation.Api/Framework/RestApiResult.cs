using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Transportation.Api.Framework
{
    public class RestApiResult
    {
        public JToken Json { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public RestApiResult()
        {
            StatusCode = HttpStatusCode.OK;
        }
    }
}