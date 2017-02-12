using Transportation.Api.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Transportation.UnitTests.Api.Framework
{
    public class ParameterParserFixture : UnitFixture
    {
        private ApiContext BuildContext(string requestUrl, string routeUrl)
        {
            var uri = new Uri(requestUrl);
            var route = new Route { Url = routeUrl };
            return new ApiContext { Uri = uri, Route = route };
        }

        [Test]
        public void Parse_NoParameters_ReturnsEmptyDictionary()
        {
            var context = BuildContext("http://localhost/", "/");
            var parameters = ParameterParser.Parse(context);
            Assert.NotNull(parameters);
            Assert.AreEqual(0, parameters.Count);
        }

        [Test]
        public void Parse_WithID_ReturnsDictionaryWithParameter()
        {
            var context = BuildContext("http://localhost/test/1", "/test/{id}");
            var parameters = ParameterParser.Parse(context);
            ApiAssert.ContainsParameter(parameters, "id", 1);
        }

        [Test]
        public void Parse_WithInvalidID_ThrowsException()
        {
            Assert.Throws<FormatException>(() => ParameterParser.Parse(BuildContext("http://localhost/test/xxx", "/test/{id}")));
        }

        [Test]
        public void Parse_WithParameter_AddsStringToDictionary()
        {
            var context = BuildContext("http://localhost/test?param=value", "/test");
            var parameters = ParameterParser.Parse(context);
            ApiAssert.ContainsParameter(parameters, "param", "value");
        }

        [Test]
        public void Parse_WithParameter_DecodesParameter()
        {
            var value = HttpUtility.UrlEncode("encoded value");
            var context = BuildContext("http://localhost/test?param=" + value, "/test");
            var parameters = ParameterParser.Parse(context);
            ApiAssert.ContainsParameter(parameters, "param", "encoded value");
        }


        [Test]
        public void Parse_WithTwoParameters_AddsStringToDictionary()
        {
            var context = BuildContext("http://localhost/test?param=value&test=text", "/test");
            var parameters = ParameterParser.Parse(context);
            ApiAssert.ContainsParameter(parameters, "param", "value");
            ApiAssert.ContainsParameter(parameters, "test", "text");
        }

        [Test]
        public void Parse_WithJsonBody_AddsJObjectToDictionary()
        {
            var context = new ApiContext { 
                Uri = new Uri("http://localhost/test"), 
                Route = new Route{ Url = "/test" }, 
                RequestBody = "{ string: 'value', integer: 1 }"
            };

            var parameters = ParameterParser.Parse(context);
            ApiAssert.ContainsParameter(parameters, "json");
        }

        [Test]
        public void Parse_WithHttpContext_AddsJObjectToDictionary()
        {
            var httpRequest = new HttpRequest("test.jpg","http://localhost/uploadImage","{}");
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);

            HttpContext httpContext = new HttpContext(httpRequest, httpResponce);

            var context = new ApiContext
            {
                Uri = new Uri("http://localhost/test"),
                Route = new Route { Url = "/test" },
                RequestBody = "{ string: 'value', integer: 1 }",
                HttpFileCollection = new Transportation.Api.Framework.HttpFileCollectionWrapper(httpContext.Request.Files)
            };

            var parameters = ParameterParser.Parse(context);
            ApiAssert.ContainsParameter(parameters, "httpFileCollection");
        }
    }
}
