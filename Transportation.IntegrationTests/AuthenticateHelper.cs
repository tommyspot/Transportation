using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Transportation.Api.Framework;

namespace Transportation.IntegrationTests
{
    public class AuthenticateHelper
    {       
        public static bool IsUnauthorized(string url, string method)
        {
            WebException exception = null;
            var request = HttpWebRequest.Create(url);
            request.Method = method;

            //  HttpWebRequest requestA = (HttpWebRequest)WebRequest.Create(url);
            //requestA.CookieContainer = new CookieContainer();
            //requestA.CookieContainer.Add(new Cookie("user", @"%7B%22username%22%3A%22User%22%2C%22isAuthenticated%22%3Atrue%7D"));


            //CredentialCache cache = new CredentialCache();
            //NetworkCredential nc = new NetworkCredential("User", "Password");
            //cache.Add(new Uri(url), "Basic", nc);
            //WebRequest requestB = (HttpWebRequest)WebRequest.Create(url);
            //string authInfo = "User:Password";
            //authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            //requestB.Headers.Add("Authorization", "Basic " + authInfo);
            //requestB.Method = method;
            //requestB.Credentials = cache;

            try
            {
                var response = request.GetResponse();
            }
            catch (WebException ex)
            {
                exception = ex;
            }

            string unauthorizedMessage = "The remote server returned an error: (401) Unauthorized.";            

            return exception != null ? unauthorizedMessage == exception.Message : false;
        }
    }
}
