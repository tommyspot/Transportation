using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;
using Transportation.Api.Framework;
using Newtonsoft.Json.Linq;

namespace Transportation.Api
{
    public class AuthenticationService
    {
        private IFormAuthenticationService formsAuthentication;
        private IPasswordHash passwordHash;
        private HttpContextBase context;

        public AuthenticationService() : this(new FormsAuthenticationService(), new PasswordHash(), new HttpContextWrapper(HttpContext.Current)) { }

        public AuthenticationService(IFormAuthenticationService formsAuthentication, IPasswordHash passwordHash, HttpContextBase context)
        {
            this.formsAuthentication = formsAuthentication;
            this.passwordHash = passwordHash;
            this.context = context;
        }

        [Route(HttpVerb.Post, "/auth/login")]
        public RestApiResult Login(JObject json)
        {
            if (json == null || json.Value<string>("username") == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.BadRequest };
            }

            string userName = json.Value<string>("username");
            User user = ClarityDB.Instance.Users.Where(x => x.UserName == userName).FirstOrDefault();

            if (user == null)
            {
                return new RestApiResult { StatusCode = HttpStatusCode.NotFound };
            }

            if (user.Password != passwordHash.CreatePasswordHash(json.Value<string>("password"), user.Salt))
            {
                return new RestApiResult { StatusCode = HttpStatusCode.Conflict };
            }

            formsAuthentication.SetAuthCookie(userName, false);

            int currentInterval = GetCurrentInterval();

            return new RestApiResult { StatusCode = HttpStatusCode.OK, Json = new JArray() { user.ToJson(), currentInterval } };
        }

        [Route(HttpVerb.Get, "/auth/logout")]
        public RestApiResult LogOut()
        {
            formsAuthentication.SignOut();
            return new RestApiResult { StatusCode = HttpStatusCode.OK };
        }

        private int GetCurrentInterval()
        {
            return DateTime.Now.Minute * 60 * 1000 + (60 - DateTime.Now.Second) * 1000;
        }
    }
}
