using Transportation.Api.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.UnitTests.Api.Framework
{
    public class RouteFixture : UnitFixture
    {
        [SetUp]
        public void SetUp()
        {
            TestService.WasInvokedFrom = null;
            TestService.WasInvokedWithID = -1;        
        }

        private Route BuildRoute(string methodName)
        {
            var method = typeof(TestService).GetMethod(methodName);
            return new Route { Method = method };
        }
        
        [Test]
        public void Invoke_MethodWithoutParameters_InvokesMethod()
        {
            var parameters = new Dictionary<string, object>();
            BuildRoute("MethodWithoutParameters").Invoke(parameters);
            Assert.AreEqual("MethodWithoutParameters", TestService.WasInvokedFrom);
        }

        [Test]
        public void Invoke_MethodWithoutParameters_ReturnsResult()
        {
            var parameters = new Dictionary<string, object>();
            var result = BuildRoute("MethodWithoutParameters").Invoke(parameters);
            Assert.IsInstanceOf<RestApiResult>(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(null, result.Json);
        }

        [Test]
        public void Invoke_MethodWithIDParameter_ReturnsResult()
        {
            var parameters = new Dictionary<string, object> { { "id", 1} };
            var result = BuildRoute("MethodWithIDParameter").Invoke(parameters);
            Assert.AreEqual(1, TestService.WasInvokedWithID);
        }

        [Test]
        public void Invoke_MethodWithQueryParameter_ReturnsResult()
        {
            var parameters = new Dictionary<string, object> { { "query", "test" } };
            var result = BuildRoute("MethodWithQueryParameter").Invoke(parameters);
            Assert.AreEqual("test", TestService.WasInvokedWithQuery);
        }

        [Test]
        public void Invoke_MethodWithParametersOutOfSequence_ReturnsResult()
        {
            var parameters = new Dictionary<string, object> { { "options", "default" }, { "query", "test" } };
            var result = BuildRoute("MethodWithQueryAndOptionsParameters").Invoke(parameters);
            Assert.AreEqual("test", TestService.WasInvokedWithQuery);
            Assert.AreEqual("default", TestService.WasInvokedWithOptions);
        }

        private class TestService
        {
            public static string WasInvokedFrom = null;
            public static long WasInvokedWithID = -1;
            public static string WasInvokedWithQuery;
            public static string WasInvokedWithOptions;

            public RestApiResult MethodWithoutParameters()
            {
                WasInvokedFrom = "MethodWithoutParameters";
                return new RestApiResult { StatusCode = HttpStatusCode.OK };
            }

            public RestApiResult MethodWithIDParameter(long id)
            {
                WasInvokedFrom = "MethodWithIDParameter";
                WasInvokedWithID = id;

                return new RestApiResult { StatusCode = HttpStatusCode.OK };
            }

            public RestApiResult MethodWithQueryParameter(string query)
            {
                WasInvokedFrom = "MethodWithIDParameter";
                WasInvokedWithQuery = query;

                return new RestApiResult { StatusCode = HttpStatusCode.OK };
            }

            public RestApiResult MethodWithQueryAndOptionsParameters(string query, string options)
            {
                WasInvokedFrom = "MethodWithIDParameter";
                WasInvokedWithQuery = query;
                WasInvokedWithOptions = options;

                return new RestApiResult { StatusCode = HttpStatusCode.OK };
            }

        }
    }
}
