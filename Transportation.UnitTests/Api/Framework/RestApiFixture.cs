using Transportation.Api.Framework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.UnitTests.Api.Framework
{
    public class RestApiFixture : UnitFixture
    {
        [SetUp]
        public void SetUp()
        {
            RestApi.Routes.Clear();
        }

        [Test]
        public void RegisterType_WithoutRoutes_RegistersNoRoutes()
        {
            RestApi.RegisterRoutes(typeof(NoRoutes));
            Assert.AreEqual(0, RestApi.Routes.Count);
        }

        [Test]
        public void RegisterType_WithRoute_RegistersARoute()
        {
            RestApi.RegisterRoutes(typeof(SingleRoute));
            Assert.AreEqual(1, RestApi.Routes.Count);
            Assert.NotNull(RestApi.Routes.First().Method);
            Assert.AreEqual("Test", RestApi.Routes.First().Method.Name);
            Assert.AreEqual("/test", RestApi.Routes.First().Url);
            Assert.AreEqual(HttpVerb.Post, RestApi.Routes.First().Verb);
        }

        [Test]
        public void RegisterAssembly_WithRoutes_RegistersRoutes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            RestApi.RegisterRoutes(assembly);
            Assert.IsTrue(RestApi.Routes.Count > 0);
        }

        private class NoRoutes 
        {
            public RestApiResult NotARoute() 
            { 
                return null; 
            }
        }

        public class SingleRoute
        {
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }
    }
}
