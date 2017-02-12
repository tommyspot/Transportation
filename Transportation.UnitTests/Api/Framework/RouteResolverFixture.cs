using Transportation.Api.Framework;
using NUnit.Framework;
using System;

namespace Transportation.UnitTests.Api.Framework
{
    public class RouteResolverFixture : UnitFixture
    {
        [SetUp]
        public void SetUp()
        {
            RestApi.Routes.Clear();
            RestApi.RoutePrefix = "";
        }

        [Test]
        public void ResolveRoute_NotFound_ReturnsNull()
        {
            var route = RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/"));
            Assert.IsNull(route);
        }

        [Test]
        public void ResolveRoute_MatchFound_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test" };
            RestApi.Routes.Add(route);
            Assert.AreSame(route, RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test/")));
        }

        [Test]
        public void ResolveRoute_MatchWithTrailingSlashFound_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/" };
            RestApi.Routes.Add(route);
            Assert.AreSame(route, RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test/")));
        }

        [Test]
        public void ResolveRoute_MatchWithTrailingSlashAndQueryString_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/" };
            RestApi.Routes.Add(route);
            Assert.AreSame(route, RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test/?query=test")));
        }

        [Test]
        public void ResolveRoute_MatchWithTrailingSlashAndPort_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/" };
            RestApi.Routes.Add(route);
            Assert.AreSame(route, RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost:80/")));
        }

        [Test]
        public void ResolveRoute_MatchWithoutTrailingSlashInRequest_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/" };
            RestApi.Routes.Add(route);
            Assert.AreSame(route, RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test")));
        }


        [Test]
        public void ResolveRoute_NoRouteWithUrl_ReturnsNull()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test" };
            RestApi.Routes.Add(route);
            Assert.IsNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/example/")));
        }

        [Test]
        public void ResolveRoute_NoRouteWithVerb_ReturnsNull()
        {
            var route = new Route { Verb = HttpVerb.Post, Url = "/test" };
            RestApi.Routes.Add(route);
            Assert.IsNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test/")));
        }

        [Test]
        public void ResolveRoute_RouteWithTwoSegments_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/something" };
            RestApi.Routes.Add(route);
            Assert.NotNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test/something/")));
        }

        [Test]
        public void ResolveRoute_RouteWithQueryString_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/" };
            RestApi.Routes.Add(route);
            Assert.NotNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test?param=1")));
        }

        [Test]
        public void ResolveRoute_RouteWithDifferentCase_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/" };
            RestApi.Routes.Add(route);
            Assert.NotNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/TEST")));
        }

        [Test]
        public void ResolveRoute_RouteWithID_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test/{id}" };
            RestApi.Routes.Add(route);
            Assert.NotNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/test/1")));
        }

        [Test]
        public void ResolveRoute_WithPrefix_ReturnsRoute()
        {
            var route = new Route { Verb = HttpVerb.Get, Url = "/test" };
            RestApi.Routes.Add(route);
            RestApi.RoutePrefix = "/api";
            Assert.NotNull(RouteResolver.Resolve(HttpVerb.Get, new Uri("http://localhost/api/test/")));
        }
    }
}
