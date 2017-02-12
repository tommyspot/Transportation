using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Conjurer;
using Transportation.Api;
using Transportation.Api.Framework;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Transportation.UnitTests.Api
{
    public class AuthenticationServiceFixture : UnitFixture
    {
        private FakeFormsAuthentication fakeFormsAuthentication;
        private AuthenticationService service;
        private User user;
        private FakeDB fakeDB;

        [SetUp]
        public void SetUp()
        {
            fakeDB = new FakeDB();
            ClarityDB.Instance = fakeDB;
            Presto.Persist<User>(x => x.UserName = "FirstUser");
            user = Presto.Persist<User>(x => x.UserName = "SecondUser");
            
            fakeFormsAuthentication = new FakeFormsAuthentication();
            service = new AuthenticationService(fakeFormsAuthentication, new FakePasswordHash(), null);
        }

        [Test]
        public void InvalidObject_ReturnsBadRequest()
        {
            var result = service.Login(new JObject());

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Test]
        public void UserNameDoesNotExist_ReturnsNotFound()
        {
            var json = new JObject();
            json["username"] = "NoSuchUser";

            var result = service.Login(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Ignore("Not require password")]
        [Test]
        public void IncorrectPassword_ReturnsConflict()
        {
            var json = new JObject();
            json["username"] = user.UserName;
            json["password"] = "NoSuchPassword";
            json["domainName"] = "localhost";

            var result = service.Login(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Conflict, result.StatusCode);
        }

        [Test]
        public void ValidUser_ResturnsOK()
        {
            var json = new JObject();
            json["username"] = user.UserName;
            json["password"] = user.Password;
            json["domainName"] = "localhost";

            var result = service.Login(json);

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public void ValidUser_SetsAuthCookies()
        {
            var json = new JObject();
            json["username"] = user.UserName;
            json["password"] = user.Password;
            json["domainName"] = "localhost";

            var result = service.Login(json);

            Assert.That(fakeFormsAuthentication.SetAuthCookieCalled);
        }

        [Test]
        public void ValidUser_ReturnsJson()
        {
            var json = new JObject();
            json["username"] = user.UserName;
            json["password"] = user.Password;
            json["domainName"] = "localhost";

            var result = service.Login(json);

            Assert.IsNotNull(result.Json);
        }

        [Test]
        public void SignOut_SignOutCalled()
        {
            var json = new JObject();
            json["username"] = user.UserName;
            json["password"] = user.Password;

            service.Login(json);

            service.LogOut();
            Assert.That(fakeFormsAuthentication.SignOutCalled);
        }

        [Test]
        public void SignOut_ReturnsOK()
        {
            var json = new JObject();
            json["username"] = user.UserName;
            json["password"] = user.Password;

            service.Login(json);

            var result = service.LogOut();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        private HttpContextBase FakeHttpContext(string ip)
        {
            Uri uri = new Uri("http://localhost");

            var serverVariables = new NameValueCollection();
            var request = new Mock<HttpRequestBase>();
            var context = new Mock<HttpContextBase>();

            request.Setup(req => req.UserHostAddress).Returns(ip);            
            request.Setup(req => req.ServerVariables).Returns(serverVariables);

            context.Setup(ctx => ctx.Request).Returns(request.Object);

            return context.Object;
        }

    }
}
