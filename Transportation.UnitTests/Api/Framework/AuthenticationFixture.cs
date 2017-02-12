using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Conjurer;
using Transportation.Api.Framework;
using NUnit.Framework;

namespace Transportation.UnitTests.Api.Framework
{
    public class AuthenticationFixture: UnitFixture
    {
        private User user;

        [SetUp]
        public void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:9090/", null),
                new HttpResponse(null));

            ClarityDB.Instance = new FakeDB();
            user = Presto.Persist<User>();
        }

        [Test]
        public void GetCurrentUser_UserIsAuthenticated_ReturnsUser()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );

            var currentUser = Authentication.GetCurrentUser();
            Assert.AreSame(user, currentUser);
        }

        [Test]
        public void GetCurrentUser_UserDoesNotExist_ReturnsNull()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket("username", false, 1)),
                new string[0]
                );

            var currentUser = Authentication.GetCurrentUser();
            Assert.IsNull(currentUser);
        }

        [Test]
        public void GetCurrentUser_UserIsNotAuthenticated_ReturnsNull()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(1, user.UserName, new DateTime(2013, 12, 1), new DateTime(2013, 12, 2), true, "")),
                new string[0]
                );
            var currentUser = Authentication.GetCurrentUser();
            Assert.IsNull(currentUser);
        }

        [Test]
        public void IsAuthenticated_NotFormsAuthentication_ReturnsFalse()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new GenericIdentity("username"),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRoute));
            var authenticated = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void IsAuthenticated_CookiesExpires_ReturnsFalse()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(1, "", new DateTime(2013, 12, 1), new DateTime(2013, 12, 2), true, "")),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRoute));
            var authenticated = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void IsAuthenticated_UserDoesNotExist_ReturnsFalse()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket("username", false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRoute));
            var authenticated = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsFalse(authenticated);
        }

        [Test]
        public void IsAuthenticated_Authenticated_ReturnsTrue()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRoute));
            var authenticated = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticated);
        }

        [Test]
        public void IsAuthenticated_WithClassAuthenticateAndHasPermission_ReturnsTrue()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithClassAuthenticateAndDoesNotHavePermission_ReturnsFalse()
        {
            user.Role = Role.Normal;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasAccountOwnerPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired == false);
        }

        [Test]
        public void IsAuthenticated_WithClassDoestNotHaveAuthenticate_ReturnsTrue()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(NoAuthenticateRoute));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateAndHasPermission_ReturnsTrue()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]

                );
            RestApi.RegisterRoutes(typeof(MethodAuthenticateRouteAndHasPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateAndDoestNotHavePermission_ReturnsFalse()
        {
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(MethodAuthenticateRouteAndHasAccountOwnerPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired == false);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateAndUserRoleIsAccountOwner_ReturnsTrue()
        {
            user.Role = Role.AccountOwner;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateNormalAndUserRoleIsSuper_ReturnsTrue()
        {
            user.Role = Role.Super;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateAccountOwnerAndUserRoleIsSuper_ReturnsFalse()
        {
            user.Role = Role.Super;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasAccountOwnerPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsFalse(authenticationRequired);
        }


        [Test]
        public void IsAuthenticated_WithMethodAuthenticateSuperAndUserRoleIsNormal_ReturnsFalse()
        {
            user.Role = Role.Normal;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasSuperPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsFalse(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateSuperAndUserRoleIsSuper_ReturnsTrue()
        {
            user.Role = Role.Super;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasSuperPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void IsAuthenticated_WithMethodAuthenticateSuperAndUserRoleIsAccountOwner_ReturnsTrue()
        {
            user.Role = Role.AccountOwner;
            HttpContext.Current.User = new GenericPrincipal(
                new FormsIdentity(new FormsAuthenticationTicket(user.UserName, false, 1)),
                new string[0]
                );
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRouteAndHasSuperPermission));
            var authenticationRequired = Authentication.IsAuthenticated(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void AuthenticateRequired_WithoutAuthenticate_ReturnsFalse()
        {
            RestApi.RegisterRoutes(typeof(NoAuthenticateRoute));
            var authenticationRequired = Authentication.RouteRequiresAuthenticate(RestApi.Routes.First());
            Assert.IsFalse(authenticationRequired);
        }

        [Test]
        public void AuthenticateRequired_WithClassAuthenticate_ReturnsTrue()
        {
            RestApi.RegisterRoutes(typeof(ClassAuthenticateRoute));
            var authenticationRequired = Authentication.RouteRequiresAuthenticate(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }

        [Test]
        public void AuthenticateRequired_WithMethodAuthenticate_ReturnsTrue()
        {
            RestApi.RegisterRoutes(typeof(MethodAuthenticateRoute));
            var authenticationRequired = Authentication.RouteRequiresAuthenticate(RestApi.Routes.First());
            Assert.IsTrue(authenticationRequired);
        }      

        private class NoAuthenticateRoute
        {
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

        [Authenticate]
        private class ClassAuthenticateRoute
        {
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

        private class MethodAuthenticateRoute
        {
            [Authenticate]
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

        [Authenticate(Role = Role.Normal)]        
        private class ClassAuthenticateRouteAndHasPermission
        {
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

        [Authenticate(Role = Role.Super)]
        private class ClassAuthenticateRouteAndHasSuperPermission
        {
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

        [Authenticate(Role = Role.AccountOwner)]
        private class ClassAuthenticateRouteAndHasAccountOwnerPermission
        {
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }
       
        private class MethodAuthenticateRouteAndHasPermission
        {
            [Authenticate(Role = Role.Normal)]
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

        private class MethodAuthenticateRouteAndHasAccountOwnerPermission
        {
            [Authenticate(Role = Role.AccountOwner)]
            [Route(HttpVerb.Post, "/test")]
            public RestApiResult Test()
            {
                return null;
            }
        }

       [TearDown]
        public void TearDown()
        {
            RestApi.Routes.Clear();
            HttpContext.Current = null;
        }
    }
}
